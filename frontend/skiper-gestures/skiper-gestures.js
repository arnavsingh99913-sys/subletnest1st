/**
 * Skiper-UI 43 Button Gestures & Cursor-Responsive Engine
 * Inspired by: https://skiper-ui.com/v1/preview/skiper43
 * 
 * Features:
 * - Magnetic 3D spring tilt & pull following cursor coordinates
 * - Dynamic cursor-following radial spotlight & specular sheen (--mouse-x, --mouse-y)
 * - Spring-damped press compression & elastic pop on release
 * - Floating Vercel-style Tooltip Pill with spring inertia and keyboard badges
 * - Interactive cursor-anchored click ripple
 * - Automatically applied to ALL existing and dynamically created buttons
 */

(function () {
  'use strict';

  // Check if pointer/hover is supported (avoid sticky hover on touch)
  const isFinePointer = window.matchMedia && window.matchMedia('(pointer: fine)').matches;

  // Singleton Tooltip Pill
  let tooltipPill = null;
  let tooltipLabel = null;
  let tooltipBadge = null;

  // Tooltip animation coordinates
  let pillCurrentX = 0;
  let pillCurrentY = 0;
  let pillTargetX = 0;
  let pillTargetY = 0;
  let pillIsVisible = false;
  let pillAnimFrame = null;
  let activeHoveredButton = null;

  // Track buttons with active magnetic loops
  const activeButtons = new WeakMap();

  // Known Shortcut Badges for SubletNest buttons
  const KNOWN_BUTTON_SHORTCUTS = {
    'obConfirmLocationBtn': { label: 'Explore Sublets', badge: 'Enter ↵' },
    'obSkipAreaBtn': { label: 'Skip Area & View All', badge: 'Space' },
    'obClearAreaBtn': { label: 'Clear Area Input', badge: 'Esc' },
    'navBtnExplore': { label: 'Browse Marketplace', badge: 'Alt+E' },
    'navBtnList': { label: 'Publish Spare Room', badge: 'Alt+L' },
    'btnShortlistToggle': { label: 'Saved Rooms', badge: '♥' },
    'btnResetFilters': { label: 'Reset All Filters', badge: 'Alt+R' },
    'btnClearAreaSearch': { label: 'Clear Search', badge: 'Esc' },
    'obLoginSubmitBtn': { label: 'Sign In to Portal', badge: 'Enter ↵' },
    'obSignupSubmitBtn': { label: 'Complete Registration', badge: 'Enter ↵' },
    'obBtnSwitchSignup': { label: 'Create New Account', badge: 'Tab' },
    'obBtnSwitchLogin': { label: 'Sign In Existing Leaser', badge: 'Tab' },
    'btnPublishListing': { label: 'Publish to SQLite Backend', badge: 'Enter ↵' },
    'btnCancelPublish': { label: 'Discard & Go Back', badge: 'Esc' },
    'adminTabBtnUsers': { label: 'Registered Leasers', badge: 'Tab 1' },
    'adminTabBtnAudit': { label: 'Listing Audit History', badge: 'Tab 2' }
  };

  /**
   * Initialize Global Tooltip Element
   */
  function initTooltipPill() {
    if (tooltipPill || typeof document === 'undefined') return;

    tooltipPill = document.createElement('div');
    tooltipPill.id = 'skiperTooltipPill';
    tooltipPill.innerHTML = `
      <span class="skiper-tooltip-label"></span>
      <span class="skiper-tooltip-badge"></span>
    `;

    document.body.appendChild(tooltipPill);
    tooltipLabel = tooltipPill.querySelector('.skiper-tooltip-label');
    tooltipBadge = tooltipPill.querySelector('.skiper-tooltip-badge');

    startTooltipAnimationLoop();
  }

  /**
   * Tooltip Spring Animation Loop (60-120fps requestAnimationFrame)
   */
  function startTooltipAnimationLoop() {
    function step() {
      if (pillIsVisible) {
        // Spring physics interpolation
        pillCurrentX += (pillTargetX - pillCurrentX) * 0.28;
        pillCurrentY += (pillTargetY - pillCurrentY) * 0.28;

        tooltipPill.style.left = `${pillCurrentX.toFixed(2)}px`;
        tooltipPill.style.top = `${pillCurrentY.toFixed(2)}px`;
      }
      pillAnimFrame = requestAnimationFrame(step);
    }
    pillAnimFrame = requestAnimationFrame(step);
  }

  /**
   * Resolve Button Label & Shortcut Badge
   */
  function getButtonTooltipMeta(btn) {
    // 1. Check known IDs
    if (btn.id && KNOWN_BUTTON_SHORTCUTS[btn.id]) {
      return KNOWN_BUTTON_SHORTCUTS[btn.id];
    }

    // 2. Check data attributes
    if (btn.getAttribute('data-skiper-tooltip')) {
      return {
        label: btn.getAttribute('data-skiper-tooltip'),
        badge: btn.getAttribute('data-skiper-badge') || 'Click'
      };
    }

    // 3. Check cached title (to prevent default browser yellow popup)
    if (btn.getAttribute('data-skiper-title')) {
      return {
        label: btn.getAttribute('data-skiper-title'),
        badge: 'Click'
      };
    }

    if (btn.hasAttribute('title') && btn.getAttribute('title')) {
      const t = btn.getAttribute('title');
      btn.setAttribute('data-skiper-title', t);
      btn.removeAttribute('title'); // Suppress native popup
      return { label: t, badge: 'Click' };
    }

    // 4. Class-based badges
    if (btn.classList.contains('btnGoogleSignIn') || btn.classList.contains('ob-btn-google')) {
      return { label: 'Continue with Google', badge: 'G' };
    }
    if (btn.classList.contains('btn-shortlist')) {
      return { label: 'Add to Saved Rooms', badge: '♥' };
    }
    if (btn.classList.contains('ob-locality-chip')) {
      return { label: `Shortlist ${btn.innerText.trim()}`, badge: '📍' };
    }
    if (btn.classList.contains('area-shortlist-chip')) {
      return { label: `Filter: ${btn.innerText.trim()}`, badge: '📍' };
    }

    // 5. Text extraction
    const rawText = btn.innerText.trim().replace(/\s+/g, ' ');
    if (rawText && rawText.length < 35) {
      return { label: rawText, badge: '↵' };
    }

    if (btn.getAttribute('aria-label')) {
      return { label: btn.getAttribute('aria-label'), badge: '↵' };
    }

    return null;
  }

  /**
   * Show Floating Spring Tooltip above button
   */
  function showTooltipForButton(btn) {
    if (!tooltipPill || !isFinePointer) return;

    const meta = getButtonTooltipMeta(btn);
    if (!meta || !meta.label) return;

    activeHoveredButton = btn;

    tooltipLabel.innerText = meta.label;
    if (meta.badge) {
      tooltipBadge.innerText = meta.badge;
      tooltipBadge.style.display = 'inline-flex';
    } else {
      tooltipBadge.style.display = 'none';
    }

    const rect = btn.getBoundingClientRect();
    const tooltipHeight = 36;
    const padding = 10;

    // Center above button
    let targetX = rect.left + rect.width / 2;
    let targetY = rect.top - padding;

    // If too close to viewport top, flip below button
    if (targetY - tooltipHeight < 10) {
      targetY = rect.bottom + tooltipHeight + padding;
      tooltipPill.classList.add('pill-bottom');
    } else {
      tooltipPill.classList.remove('pill-bottom');
    }

    // Keep within horizontal viewport boundaries
    targetX = Math.max(80, Math.min(window.innerWidth - 80, targetX));

    pillTargetX = targetX;
    pillTargetY = targetY;

    if (!pillIsVisible) {
      // First show: snap position to target so it doesn't fly in from corner
      pillCurrentX = targetX;
      pillCurrentY = targetY;
      tooltipPill.style.left = `${targetX}px`;
      tooltipPill.style.top = `${targetY}px`;
    }

    tooltipPill.classList.add('visible');
    pillIsVisible = true;
  }

  /**
   * Hide Floating Tooltip
   */
  function hideTooltipForButton(btn) {
    if (activeHoveredButton === btn) {
      activeHoveredButton = null;
      if (tooltipPill) {
        tooltipPill.classList.remove('visible');
        pillIsVisible = false;
      }
    }
  }

  /**
   * Attach Skiper43 Gestures to a Single Button Element
   */
  function attachSkiperGestures(btn) {
    if (btn.dataset.skiperAttached === 'true') return;
    btn.dataset.skiperAttached = 'true';

    // Cache title if exists
    if (btn.hasAttribute('title') && btn.getAttribute('title')) {
      btn.setAttribute('data-skiper-title', btn.getAttribute('title'));
      btn.removeAttribute('title');
    }

    // Ensure spotlight overlay exists
    let spotlight = btn.querySelector('.skiper-spotlight-overlay');
    if (!spotlight) {
      spotlight = document.createElement('span');
      spotlight.className = 'skiper-spotlight-overlay';
      btn.appendChild(spotlight);
    }

    // State for magnetic motion
    const state = {
      isHovered: false,
      isPressed: false,
      currentX: 0,
      currentY: 0,
      currentTiltX: 0,
      currentTiltY: 0,
      targetX: 0,
      targetY: 0,
      targetTiltX: 0,
      targetTiltY: 0,
      rafId: null
    };
    activeButtons.set(btn, state);

    function springAnimationStep() {
      // Damped spring interpolation
      state.currentX += (state.targetX - state.currentX) * 0.22;
      state.currentY += (state.targetY - state.currentY) * 0.22;
      state.currentTiltX += (state.targetTiltX - state.currentTiltX) * 0.22;
      state.currentTiltY += (state.targetTiltY - state.currentTiltY) * 0.22;

      const scale = state.isPressed ? 0.94 : (state.isHovered ? 1.025 : 1.0);

      btn.style.transform = `perspective(600px) translate3d(${state.currentX.toFixed(2)}px, ${state.currentY.toFixed(2)}px, 0) rotateX(${state.currentTiltX.toFixed(2)}deg) rotateY(${state.currentTiltY.toFixed(2)}deg) scale(${scale})`;

      // Continue animating until settled
      const dist = Math.abs(state.targetX - state.currentX) +
                   Math.abs(state.targetY - state.currentY) +
                   Math.abs(state.targetTiltX - state.currentTiltX) +
                   Math.abs(state.targetTiltY - state.currentTiltY);

      if (state.isHovered || dist > 0.05) {
        state.rafId = requestAnimationFrame(springAnimationStep);
      } else {
        // Snap to zero when settled
        btn.style.transform = '';
        state.rafId = null;
      }
    }

    function ensureSpringLoop() {
      if (!state.rafId) {
        state.rafId = requestAnimationFrame(springAnimationStep);
      }
    }

    // Pointer Enter
    btn.addEventListener('pointerenter', (e) => {
      state.isHovered = true;
      btn.classList.add('skiper-active');

      if (isFinePointer) {
        showTooltipForButton(btn);
      }
      ensureSpringLoop();
    });

    // Pointer Move (Magnetic Attraction + Cursor Spotlight)
    btn.addEventListener('pointermove', (e) => {
      const rect = btn.getBoundingClientRect();
      const x = e.clientX - rect.left;
      const y = e.clientY - rect.top;

      // Update cursor coordinates for CSS radial spotlight
      btn.style.setProperty('--mouse-x', `${x.toFixed(1)}px`);
      btn.style.setProperty('--mouse-y', `${y.toFixed(1)}px`);

      if (!isFinePointer) return;

      // Calculate magnetic displacement from center (-1 to 1)
      const centerX = rect.width / 2;
      const centerY = rect.height / 2;
      const deltaX = (x - centerX) / (centerX || 1);
      const deltaY = (y - centerY) / (centerY || 1);

      // Max magnetic translation: 4px to 7px depending on button size
      const maxDisplacement = Math.min(7, Math.max(3, rect.width * 0.06));
      state.targetX = deltaX * maxDisplacement;
      state.targetY = deltaY * maxDisplacement;

      // 3D subtle tilt
      state.targetTiltX = -deltaY * 6;
      state.targetTiltY = deltaX * 6;

      ensureSpringLoop();
    });

    // Pointer Leave
    btn.addEventListener('pointerleave', () => {
      state.isHovered = false;
      state.isPressed = false;
      state.targetX = 0;
      state.targetY = 0;
      state.targetTiltX = 0;
      state.targetTiltY = 0;

      btn.classList.remove('skiper-active');
      btn.style.removeProperty('--mouse-x');
      btn.style.removeProperty('--mouse-y');

      hideTooltipForButton(btn);
      ensureSpringLoop();
    });

    // Pointer Down (Press Spring Compression)
    btn.addEventListener('pointerdown', (e) => {
      state.isPressed = true;
      ensureSpringLoop();
      createSkiperRipple(btn, e);
    });

    // Pointer Up (Elastic Pop Release)
    btn.addEventListener('pointerup', () => {
      state.isPressed = false;
      ensureSpringLoop();
    });
  }

  /**
   * Create Dynamic Ripple from Cursor Coordinates
   */
  function createSkiperRipple(btn, e) {
    const rect = btn.getBoundingClientRect();
    const x = e.clientX - rect.left;
    const y = e.clientY - rect.top;

    const size = Math.max(rect.width, rect.height) * 1.5;
    const ripple = document.createElement('span');
    ripple.className = 'skiper-ripple-effect';
    ripple.style.width = `${size}px`;
    ripple.style.height = `${size}px`;
    ripple.style.left = `${x - size / 2}px`;
    ripple.style.top = `${y - size / 2}px`;

    btn.appendChild(ripple);

    setTimeout(() => {
      if (ripple && ripple.parentNode === btn) {
        btn.removeChild(ripple);
      }
    }, 600);
  }

  /**
   * Scan & Attach to All Buttons in Document
   */
  function scanAndAttachAllButtons(root = document) {
    if (!root || !root.querySelectorAll) return;

    const selector = `
      button,
      a[role="button"],
      .ob-btn-primary,
      .ob-btn-google,
      .ob-locality-chip,
      .area-shortlist-chip,
      .btn-shortlist,
      .main-app-location-badge,
      input[type="submit"]
    `;

    const elements = root.querySelectorAll(selector);
    elements.forEach(el => attachSkiperGestures(el));
  }

  /**
   * Initialize Global Skiper Gestures Engine
   */
  function initSkiperGestures() {
    initTooltipPill();
    scanAndAttachAllButtons(document);

    // MutationObserver to automatically attach to dynamically rendered buttons (listings, filters, modals)
    const observer = new MutationObserver((mutations) => {
      for (const m of mutations) {
        if (m.addedNodes && m.addedNodes.length > 0) {
          m.addedNodes.forEach(node => {
            if (node.nodeType === Node.ELEMENT_NODE) {
              if (node.matches && node.matches('button, a[role="button"], .ob-locality-chip, .area-shortlist-chip, .btn-shortlist')) {
                attachSkiperGestures(node);
              }
              scanAndAttachAllButtons(node);
            }
          });
        }
      }
    });

    observer.observe(document.body, { childList: true, subtree: true });

    // Hide tooltip on scroll to maintain seamless feel
    window.addEventListener('scroll', () => {
      if (tooltipPill && pillIsVisible) {
        tooltipPill.classList.remove('visible');
        pillIsVisible = false;
      }
    }, { passive: true });
  }

  // Window API
  window.SkiperGestures = {
    init: initSkiperGestures,
    attach: attachSkiperGestures,
    scanAll: scanAndAttachAllButtons
  };

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initSkiperGestures);
  } else {
    initSkiperGestures();
  }

})();


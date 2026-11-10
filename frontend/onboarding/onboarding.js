/**
 * SubletNest Experience Engine
 * - First Slide: Pure dropdown-driven selection of All 28 States & 8 Union Territories in India
 * - Cascading Cities dropdown for every State and UT
 * - Area / Street / Locality selection (Optional & Skippable) with 1-click popular locality chips
 * - Multi-keyword address shortlisting: street name, city name, state name, neighborhood
 * - Shifted Leaser Authentication: opened when listing a room or managing profile
 * - Google Sign-In & Password Auth (Name, Email, Contact + Password)
 * - Admin Portal: arnavsingh99913@gmail.com / Arnav$123
 */

(function() {
  'use strict';

  // =========================================================================
  // 1. COMPREHENSIVE ALL-INDIA STATES & CITIES DATA REGISTRY (36 States & UTs)
  // =========================================================================
  const INDIA_STATES_AND_CITIES = [
    {
      state: "Andhra Pradesh",
      code: "AP",
      cities: [
        "Visakhapatnam", "Vijayawada", "Guntur", "Nellore", "Kurnool", "Rajahmundry",
        "Tirupati", "Kakinada", "Kadapa", "Anantapur", "Vizianagaram", "Eluru", "Ongole",
        "Nandyal", "Machilipatnam", "Tenali", "Chittoor", "Hindupur", "Bhimavaram"
      ]
    },
    {
      state: "Arunachal Pradesh",
      code: "AR",
      cities: ["Itanagar", "Naharlagun", "Pasighat", "Tawang", "Ziro", "Along", "Tezu", "Bomdila"]
    },
    {
      state: "Assam",
      code: "AS",
      cities: [
        "Guwahati", "Silchar", "Dibrugarh", "Jorhat", "Nagaon", "Tinsukia", "Tezpur",
        "Bongaigaon", "Karimganj", "Sivasagar", "Goalpara", "Barpeta"
      ]
    },
    {
      state: "Bihar",
      code: "BR",
      cities: [
        "Patna", "Gaya", "Bhagalpur", "Muzaffarpur", "Purnia", "Darbhanga", "Bihar Sharif",
        "Arrah", "Begusarai", "Katihar", "Munger", "Chhapra", "Danapur", "Saharsa",
        "Sasaram", "Hajipur", "Dehri", "Siwan", "Motihari", "Nawada"
      ]
    },
    {
      state: "Chhattisgarh",
      code: "CG",
      cities: [
        "Raipur", "Bhilai", "Bilaspur", "Korba", "Durg", "Rajnandgaon", "Jagdalpur",
        "Raigarh", "Ambikapur", "Dhamtari", "Mahasamund"
      ]
    },
    {
      state: "Goa",
      code: "GA",
      cities: ["Panaji", "Margao", "Vasco da Gama", "Mapusa", "Ponda", "Candolim", "Calangute", "Porvorim", "Bicholim"]
    },
    {
      state: "Gujarat",
      code: "GJ",
      cities: [
        "Ahmedabad", "Surat", "Vadodara (Baroda)", "Rajkot", "Bhavnagar", "Jamnagar",
        "Gandhinagar", "Junagadh", "Anand", "Navsari", "Morbi", "Bharuch", "Porbandar",
        "Mehsana", "Bhuj", "Vapi", "Valsad", "Gondal", "Palanpur", "Godhra"
      ]
    },
    {
      state: "Haryana",
      code: "HR",
      cities: [
        "Gurugram (Gurgaon)", "Faridabad", "Panipat", "Ambala", "Yamunanagar", "Rohtak",
        "Hisar", "Karnal", "Sonipat", "Panchkula", "Sirsa", "Bhiwani", "Bahadurgarh",
        "Jind", "Thanesar", "Kaithal", "Rewari", "Palwal"
      ]
    },
    {
      state: "Himachal Pradesh",
      code: "HP",
      cities: ["Shimla", "Dharamshala", "Solan", "Mandi", "Kullu", "Manali", "Baddi", "Palampur", "Nahan", "Una", "Bilaspur"]
    },
    {
      state: "Jharkhand",
      code: "JH",
      cities: [
        "Ranchi", "Jamshedpur", "Dhanbad", "Bokaro Steel City", "Deoghar", "Hazaribagh",
        "Giridih", "Ramgarh", "Medininagar", "Phusro", "Chirkunda"
      ]
    },
    {
      state: "Karnataka",
      code: "KA",
      cities: [
        "Bengaluru (Bangalore)", "Mysuru (Mysore)", "Mangaluru (Mangalore)", "Hubballi-Dharwad",
        "Belagavi (Belgaum)", "Davanagere", "Ballari (Bellary)", "Shivamogga (Shimoga)",
        "Tumakuru (Tumkur)", "Udupi", "Kalaburagi (Gulbarga)", "Hassan", "Bidar",
        "Raichur", "Hosapete", "Chikkamagaluru", "Mandya", "Kolar"
      ]
    },
    {
      state: "Kerala",
      code: "KL",
      cities: [
        "Thiruvananthapuram (Trivandrum)", "Kochi (Cochin)", "Kozhikode (Calicut)",
        "Thrissur", "Kollam", "Palakkad", "Alappuzha", "Kannur", "Kottayam", "Malappuram",
        "Kasaragod", "Thalassery", "Ponnani", "Manjeri"
      ]
    },
    {
      state: "Madhya Pradesh",
      code: "MP",
      cities: [
        "Indore", "Bhopal", "Jabalpur", "Gwalior", "Ujjain", "Sagar", "Dewas", "Satna",
        "Ratlam", "Rewa", "Katni", "Singrauli", "Burhanpur", "Khandwa", "Bhind", "Chhindwara"
      ]
    },
    {
      state: "Maharashtra",
      code: "MH",
      cities: [
        "Mumbai", "Pune", "Nagpur", "Thane", "Nashik", "Navi Mumbai", "Chhatrapati Sambhajinagar (Aurangabad)",
        "Solapur", "Kalyan-Dombivli", "Vasai-Virar", "Pimpri-Chinchwad", "Kolhapur", "Amravati",
        "Nanded", "Sangli", "Jalgaon", "Akola", "Latur", "Dhule", "Ahmednagar", "Chandrapur", "Panvel"
      ]
    },
    {
      state: "Manipur",
      code: "MN",
      cities: ["Imphal", "Churachandpur", "Thoubal", "Kakching", "Ukhrul"]
    },
    {
      state: "Meghalaya",
      code: "ML",
      cities: ["Shillong", "Tura", "Jowai", "Nongpoh", "Cherrapunji"]
    },
    {
      state: "Mizoram",
      code: "MZ",
      cities: ["Aizawl", "Lunglei", "Champhai", "Serchhip", "Kolasib"]
    },
    {
      state: "Nagaland",
      code: "NL",
      cities: ["Kohima", "Dimapur", "Mokokchung", "Tuensang", "Wokha"]
    },
    {
      state: "Odisha",
      code: "OD",
      cities: [
        "Bhubaneswar", "Cuttack", "Rourkela", "Berhampur", "Sambalpur", "Puri",
        "Balasore", "Bhadrak", "Baripada", "Jharsuguda", "Jeypore"
      ]
    },
    {
      state: "Punjab",
      code: "PB",
      cities: [
        "Ludhiana", "Amritsar", "Jalandhar", "Patiala", "Bathinda", "Mohali (SAS Nagar)",
        "Hoshiarpur", "Pathankot", "Moga", "Batala", "Abohar", "Khanna", "Phagwara"
      ]
    },
    {
      state: "Rajasthan",
      code: "RJ",
      cities: [
        "Jaipur", "Jodhpur", "Kota", "Bikaner", "Ajmer", "Udaipur", "Bhilwara",
        "Alwar", "Bharatpur", "Sikar", "Pali", "Sri Ganganagar", "Hanumangarh", "Chittorgarh"
      ]
    },
    {
      state: "Sikkim",
      code: "SK",
      cities: ["Gangtok", "Namchi", "Geyzing", "Mangan", "Rangpo"]
    },
    {
      state: "Tamil Nadu",
      code: "TN",
      cities: [
        "Chennai", "Coimbatore", "Madurai", "Tiruchirappalli (Trichy)", "Salem",
        "Tiruppur", "Erode", "Vellore", "Tirunelveli", "Thoothukudi", "Thanjavur",
        "Dindigul", "Hosur", "Nagercoil", "Kanchipuram", "Kumbakonam"
      ]
    },
    {
      state: "Telangana",
      code: "TS",
      cities: [
        "Hyderabad", "Secunderabad", "Warangal", "Nizamabad", "Karimnagar", "Ramagundam",
        "Khammam", "Mahbubnagar", "Nalgonda", "Adilabad", "Suryapet", "Siddipet"
      ]
    },
    {
      state: "Tripura",
      code: "TR",
      cities: ["Agartala", "Dharmanagar", "Udaipur", "Kailashahar", "Belonia"]
    },
    {
      state: "Uttar Pradesh",
      code: "UP",
      cities: [
        "Lucknow", "Kanpur", "Noida", "Greater Noida", "Ghaziabad", "Agra", "Varanasi (Banaras)",
        "Prayagraj (Allahabad)", "Meerut", "Bareilly", "Aligarh", "Moradabad", "Saharanpur",
        "Gorakhpur", "Jhansi", "Muzaffarnagar", "Mathura", "Ayodhya", "Firozabad"
      ]
    },
    {
      state: "Uttarakhand",
      code: "UK",
      cities: ["Dehradun", "Haridwar", "Roorkee", "Haldwani", "Rishikesh", "Nainital", "Rudrapur", "Mussoorie"]
    },
    {
      state: "West Bengal",
      code: "WB",
      cities: [
        "Kolkata", "Howrah", "Durgapur", "Asansol", "Siliguri", "Bardhaman", "Malda",
        "Kharagpur", "Haldia", "Baharampur", "Habra", "Kalyani", "Darjeeling"
      ]
    },
    {
      state: "Andaman and Nicobar Islands",
      code: "AN",
      cities: ["Port Blair", "Diglipur", "Mayabunder"]
    },
    {
      state: "Chandigarh",
      code: "CH",
      cities: ["Chandigarh"]
    },
    {
      state: "Dadra and Nagar Haveli and Daman and Diu",
      code: "DN",
      cities: ["Daman", "Diu", "Silvassa"]
    },
    {
      state: "Delhi (NCT)",
      code: "DL",
      cities: [
        "New Delhi", "South Delhi", "Central Delhi", "North Delhi", "West Delhi", "East Delhi",
        "Dwarka", "Rohini", "Saket", "Connaught Place", "Hauz Khas", "Vasant Kunj", "Lajpat Nagar", "Janakpuri"
      ]
    },
    {
      state: "Jammu and Kashmir",
      code: "JK",
      cities: ["Srinagar", "Jammu", "Anantnag", "Baramulla", "Udhampur", "Kathua"]
    },
    {
      state: "Ladakh",
      code: "LA",
      cities: ["Leh", "Kargil"]
    },
    {
      state: "Lakshadweep",
      code: "LD",
      cities: ["Kavaratti", "Agatti", "Andrott"]
    },
    {
      state: "Puducherry",
      code: "PY",
      cities: ["Puducherry (Pondicherry)", "Karaikal", "Mahe", "Yanam", "Auroville"]
    }
  ];

  // Popular Localities & Neighborhoods for 1-Click Shortlisting
  const POPULAR_LOCALITIES_BY_CITY = {
    "bengaluru": ["Indiranagar", "Koramangala", "HSR Layout", "Whitefield", "Bellandur", "BTM Layout", "Jayanagar", "Marathahalli", "Electronic City", "Sarjapur Road", "Hebbal", "JP Nagar"],
    "bangalore": ["Indiranagar", "Koramangala", "HSR Layout", "Whitefield", "Bellandur", "BTM Layout", "Jayanagar", "Marathahalli", "Electronic City", "Sarjapur Road", "Hebbal", "JP Nagar"],
    "mumbai": ["Bandra West", "Andheri West", "Powai", "Juhu", "Worli", "Lower Parel", "Dadar", "Goregaon", "Malad", "Khar West", "Versova", "Chembur"],
    "pune": ["Koregaon Park", "Hinjawadi", "Viman Nagar", "Kothrud", "Baner", "Wakad", "Aundh", "Magarpatta", "Kalyani Nagar", "Kharadi"],
    "gurgaon": ["Cyber City", "Golf Course Road", "DLF Phase 1-5", "Sohna Road", "Sector 29", "Sector 56", "MG Road", "Sector 43"],
    "gurugram": ["Cyber City", "Golf Course Road", "DLF Phase 1-5", "Sohna Road", "Sector 29", "Sector 56", "MG Road", "Sector 43"],
    "hyderabad": ["Hitec City", "Gachibowli", "Madhapur", "Jubilee Hills", "Banjara Hills", "Kondapur", "Kukatpally", "Financial District"],
    "delhi": ["Hauz Khas", "Saket", "Connaught Place", "Dwarka", "Rohini", "Vasant Kunj", "Lajpat Nagar", "Greater Kailash", "Green Park"],
    "new delhi": ["Hauz Khas", "Saket", "Connaught Place", "Dwarka", "Rohini", "Vasant Kunj", "Lajpat Nagar", "Greater Kailash", "Green Park"],
    "noida": ["Sector 62", "Sector 18", "Sector 137", "Sector 75", "Sector 50", "Noida Expressway", "Sector 15"],
    "chennai": ["OMR", "Velachery", "Adyar", "Anna Nagar", "T. Nagar", "Thiruvanmiyur", "Besant Nagar", "Guindy"],
    "kolkata": ["Salt Lake", "New Town", "Park Street", "Ballygunge", "Alipore", "Rajarhat"],
    "ahmedabad": ["Bodakdev", "Satellite", "SG Highway", "Navrangpura", "Vastrapur", "Prahlad Nagar"]
  };

  // State Management
  let selectedState = null;
  let selectedCity = null;
  let selectedArea = "";
  let pendingAuthRedirect = null;

  // Cache elements
  let elContainer = null;
  let elStepLocation = null;
  let elMainApp = null;
  let elAuthModal = null;

  // =========================================================================
  // 2. ENTRY POINT & STAGE SWITCHING
  // =========================================================================
  function initOnboarding() {
    elContainer = document.getElementById('onboardingContainer');
    elStepLocation = document.getElementById('step-location');
    elMainApp = document.getElementById('mainAppContainer');
    elAuthModal = document.getElementById('obAuthModal');

    populateStateDropdown();
    setupAuthModalEvents();

    // Check if location is already saved in localStorage
    let location = null;
    try {
      const stored = localStorage.getItem('subletnest_location');
      if (stored) location = JSON.parse(stored);
    } catch(e) { location = null; }

    if (!location) {
      showLocationRadar();
    } else {
      showMarketplace();
    }
  }

  function showLocationRadar() {
    if (elContainer) elContainer.classList.remove('ob-hidden');
    if (elStepLocation) {
      elStepLocation.classList.remove('stage-hidden');
      elStepLocation.classList.add('stage-active');
    }
    if (elMainApp) elMainApp.classList.add('opacity-0', 'pointer-events-none');

    updateProgressHUD(1);
    if (window.lucide) window.lucide.createIcons();
  }

  function showMarketplace() {
    if (elStepLocation) elStepLocation.classList.add('stage-hidden');
    if (elContainer) elContainer.classList.add('ob-hidden');
    if (elMainApp) elMainApp.classList.remove('opacity-0', 'pointer-events-none');

    updateProgressHUD(2);
    applyLocationToMainSite();

    if (typeof window.renderAuthNav === 'function') {
      window.renderAuthNav();
    }
    if (window.lucide) window.lucide.createIcons();
  }

  function updateProgressHUD(step) {
    const nodes = document.querySelectorAll('.ob-step-node');
    const connectors = document.querySelectorAll('.ob-step-connector');

    nodes.forEach(node => {
      const s = parseInt(node.getAttribute('data-step'));
      node.classList.remove('active', 'completed');
      if (s === step) node.classList.add('active');
      else if (s < step) node.classList.add('completed');
    });

    connectors.forEach((conn, idx) => {
      conn.classList.remove('completed');
      if (idx + 1 < step) conn.classList.add('completed');
    });
  }

  // =========================================================================
  // 3. DROPDOWN CONTROLS: ALL STATES & CITIES IN INDIA
  // =========================================================================
  function populateStateDropdown() {
    const stateSelect = document.getElementById('obStateDropdown');
    if (!stateSelect) return;

    // Sort states alphabetically
    const sorted = [...INDIA_STATES_AND_CITIES].sort((a, b) => a.state.localeCompare(b.state));
    stateSelect.innerHTML = `<option value="">-- 📍 Choose State or UT (36 Available) --</option>` +
      sorted.map(s => `<option value="${s.state}">📍 ${s.state}</option>`).join('');
  }

  function handleStateDropdownChange(stateName) {
    if (!stateName) {
      resetLocationForm();
      return;
    }

    const stateObj = INDIA_STATES_AND_CITIES.find(s => s.state.toLowerCase() === stateName.toLowerCase());
    if (!stateObj) return;

    selectedState = stateObj.state;
    selectedCity = null;
    selectedArea = "";

    // Populate City dropdown
    const citySelect = document.getElementById('obCityDropdown');
    if (citySelect) {
      citySelect.disabled = false;
      const sortedCities = [...stateObj.cities].sort((a, b) => a.localeCompare(b));
      citySelect.innerHTML = `<option value="">-- 🏙️ 2. Select City in ${stateObj.state} (${sortedCities.length} available) --</option>` +
        sortedCities.map(c => `<option value="${c}">🏙️ ${c}</option>`).join('');
    }

    // Disable Area input and hide chips until city is chosen
    const areaInput = document.getElementById('obAreaInput');
    if (areaInput) {
      areaInput.value = "";
      areaInput.disabled = true;
    }
    const chipsContainer = document.getElementById('obLocalityChipsContainer');
    if (chipsContainer) chipsContainer.classList.add('hidden');
    const skipBtn = document.getElementById('obSkipAreaBtn');
    if (skipBtn) skipBtn.classList.add('hidden');

    // Update Summary & Buttons
    updateSummaryText(`State selected: ${stateObj.state}. Please choose your city in dropdown 2.`);
    disableConfirmButton(`Select City in ${stateObj.state}`);
  }

  function handleCityDropdownChange(cityName) {
    if (!cityName) {
      if (selectedState) {
        handleStateDropdownChange(selectedState);
      } else {
        resetLocationForm();
      }
      return;
    }

    selectedCity = cityName;
    selectedArea = "";

    // Enable Area input
    const areaInput = document.getElementById('obAreaInput');
    if (areaInput) {
      areaInput.disabled = false;
      areaInput.value = "";
    }

    // Populate locality suggestion chips for chosen city
    renderCityLocalityChips(cityName);

    // Reveal Skip Area button
    const skipBtn = document.getElementById('obSkipAreaBtn');
    if (skipBtn) {
      skipBtn.disabled = false;
      skipBtn.classList.remove('hidden');
    }

    // Enable primary confirm button
    const cleanCity = cityName.replace(/\(.*?\)/g, '').trim();
    enableConfirmButton(`Explore Sublets in ${cleanCity} →`);
    updateSummaryText(`City locked: ${cityName}, ${selectedState}. Specify an area or street, or click explore directly.`);
  }

  function renderCityLocalityChips(cityName) {
    const chipsContainer = document.getElementById('obLocalityChipsContainer');
    const chipsList = document.getElementById('obLocalityChipsList');
    const chipsCityName = document.getElementById('obChipsCityName');

    if (!chipsContainer || !chipsList) return;

    const cleanCity = cityName.toLowerCase().replace(/\(.*?\)/g, '').trim();
    const localities = POPULAR_LOCALITIES_BY_CITY[cleanCity] || [];

    if (localities.length > 0) {
      if (chipsCityName) chipsCityName.innerText = cityName;
      chipsList.innerHTML = localities.map(loc => `
        <button 
          type="button" 
          onclick="window.onboardingManager.selectLocalityChip('${loc}')"
          class="ob-locality-chip">
          <span>${loc}</span>
        </button>
      `).join('');
      chipsContainer.classList.remove('hidden');
    } else {
      chipsContainer.classList.add('hidden');
      chipsList.innerHTML = '';
    }
  }

  function handleAreaInputChange(areaVal) {
    selectedArea = (areaVal || "").trim();

    // Toggle clear button
    const clearBtn = document.getElementById('obClearAreaBtn');
    if (clearBtn) {
      if (selectedArea) clearBtn.classList.remove('hidden');
      else clearBtn.classList.add('hidden');
    }

    // Highlight chips if area matches any
    const chips = document.querySelectorAll('.ob-locality-chip');
    chips.forEach(chip => {
      const txt = chip.innerText.trim().toLowerCase();
      if (selectedArea && txt.includes(selectedArea.toLowerCase())) {
        chip.classList.add('active');
      } else {
        chip.classList.remove('active');
      }
    });

    if (!selectedCity) return;
    const cleanCity = selectedCity.replace(/\(.*?\)/g, '').trim();

    if (selectedArea) {
      updateSummaryText(`Shortlisting rooms in ${cleanCity} matching "${selectedArea}"`);
      enableConfirmButton(`Explore Sublets in ${cleanCity} (${selectedArea}) →`);
    } else {
      updateSummaryText(`City locked: ${selectedCity}, ${selectedState}. Exploring all available rooms.`);
      enableConfirmButton(`Explore All ${cleanCity} Sublets →`);
    }
  }

  function selectLocalityChip(areaName) {
    const areaInput = document.getElementById('obAreaInput');
    if (areaInput) {
      areaInput.value = areaName;
    }
    handleAreaInputChange(areaName);
  }

  function clearAreaInput() {
    const areaInput = document.getElementById('obAreaInput');
    if (areaInput) {
      areaInput.value = "";
    }
    handleAreaInputChange("");
  }

  function resetLocationForm() {
    selectedState = null;
    selectedCity = null;
    selectedArea = "";

    const stateSelect = document.getElementById('obStateDropdown');
    if (stateSelect) stateSelect.value = "";

    const citySelect = document.getElementById('obCityDropdown');
    if (citySelect) {
      citySelect.disabled = true;
      citySelect.innerHTML = `<option value="">-- 🏙️ Select State First --</option>`;
    }

    const areaInput = document.getElementById('obAreaInput');
    if (areaInput) {
      areaInput.disabled = true;
      areaInput.value = "";
    }

    const chipsContainer = document.getElementById('obLocalityChipsContainer');
    if (chipsContainer) chipsContainer.classList.add('hidden');

    const skipBtn = document.getElementById('obSkipAreaBtn');
    if (skipBtn) skipBtn.classList.add('hidden');

    updateSummaryText("Choose a State and City above to explore available rooms.");
    disableConfirmButton("Select City to Explore Sublets");
  }

  function updateSummaryText(text) {
    const summaryEl = document.getElementById('obSelectionSummary');
    if (summaryEl && text) summaryEl.innerText = text;
  }

  function enableConfirmButton(label) {
    const btn = document.getElementById('obConfirmLocationBtn');
    const textEl = document.getElementById('obConfirmBtnText');
    if (btn) {
      btn.disabled = false;
      btn.classList.remove('opacity-40', 'cursor-not-allowed');
      btn.classList.add('shadow-[0_0_20px_#38bdf8]');
      if (textEl && label) textEl.innerText = label;
    }
  }

  function disableConfirmButton(label) {
    const btn = document.getElementById('obConfirmLocationBtn');
    const textEl = document.getElementById('obConfirmBtnText');
    if (btn) {
      btn.disabled = true;
      btn.classList.add('opacity-40', 'cursor-not-allowed');
      btn.classList.remove('shadow-[0_0_20px_#38bdf8]');
      if (textEl) textEl.innerText = label || "Select City to Explore Sublets";
    }
  }

  // =========================================================================
  // 4. CONFIRM & SYNC LOCATION WITH MAIN SITE & KEYWORD SHORTLISTING
  // =========================================================================
  function confirmLocationAndLaunch(skipArea = false) {
    if (!selectedCity) return;

    const areaVal = skipArea ? "" : (selectedArea || "");
    const cleanCity = selectedCity.replace(/\(.*?\)/g, '').trim();

    const locationPayload = {
      state: selectedState || "Karnataka",
      city: cleanCity,
      rawCity: selectedCity,
      area: areaVal,
      timestamp: Date.now()
    };

    localStorage.setItem('subletnest_location', JSON.stringify(locationPayload));
    showMarketplace();
  }

  function applyLocationToMainSite() {
    let location = null;
    try {
      const stored = localStorage.getItem('subletnest_location');
      if (stored) location = JSON.parse(stored);
    } catch(e){}

    if (!location) return;

    // 1. Sync city dropdown in filter bar
    const filterCity = document.getElementById('filterCity');
    if (filterCity) {
      let optionExists = false;
      for (let opt of filterCity.options) {
        if (opt.value.toLowerCase() === location.city.toLowerCase() || 
            (opt.value !== 'all' && location.city.toLowerCase().includes(opt.value.toLowerCase()))) {
          filterCity.value = opt.value;
          optionExists = true;
          break;
        }
      }
      if (!optionExists) {
        const newOpt = document.createElement('option');
        newOpt.value = location.city;
        newOpt.innerText = location.city;
        filterCity.appendChild(newOpt);
        filterCity.value = location.city;
      }
    }

    // 2. Sync search input with selected Area / Street / Locality
    const areaInput = document.getElementById('areaSchemeSearchInput');
    const globalInput = document.getElementById('globalSearchInput');
    const clearBtn = document.getElementById('btnClearAreaSearch');

    if (location.area) {
      if (areaInput) areaInput.value = location.area;
      if (globalInput) globalInput.value = location.area;
      if (clearBtn) clearBtn.classList.remove('hidden');
    } else {
      if (areaInput) areaInput.value = "";
      if (globalInput) globalInput.value = "";
      if (clearBtn) clearBtn.classList.add('hidden');
    }

    // 3. Render Navbar Location Badge
    renderNavbarLocationBadge(location);

    // 4. Trigger marketplace keyword address shortlisting & filtering
    if (typeof window.applyLocalFiltersAndRender === 'function') {
      window.applyLocalFiltersAndRender();
    } else if (typeof window.fetchMarketplaceListings === 'function') {
      window.fetchMarketplaceListings();
    }
  }

  function renderNavbarLocationBadge(location) {
    let badgeContainer = document.getElementById('mainNavbarLocationPill');
    if (!badgeContainer) {
      const headerBar = document.querySelector('header .max-w-7xl');
      if (headerBar) {
        badgeContainer = document.createElement('div');
        badgeContainer.id = 'mainNavbarLocationPill';
        badgeContainer.className = 'flex items-center';
        const logo = headerBar.firstElementChild;
        if (logo && logo.nextSibling) {
          headerBar.insertBefore(badgeContainer, logo.nextSibling);
        } else {
          headerBar.appendChild(badgeContainer);
        }
      }
    }

    if (!badgeContainer) return;

    const locText = location.area ? `${location.city}, ${location.area}` : location.city;
    badgeContainer.innerHTML = `
      <div 
        onclick="window.onboardingManager.reopenLocationRadar()" 
        title="Click to change Indian State or City"
        class="main-app-location-badge ml-1 sm:ml-3">
        <span class="loc-dot animate-pulse"></span>
        <i data-lucide="map-pin" class="w-3.5 h-3.5 text-brand-600"></i>
        <span class="truncate max-w-[120px] sm:max-w-[200px] text-slate-800 font-bold">${locText}</span>
        <span class="text-[10px] text-brand-600 font-semibold hover:underline">[Change]</span>
      </div>
    `;

    if (window.lucide) window.lucide.createIcons();
  }

  function reopenLocationRadar() {
    showLocationRadar();
  }

  // =========================================================================
  // 5. SHIFTED AUTHENTICATION: OPENED WHEN LISTING OR MANAGING ROOMS
  // =========================================================================
  function openAuthModal(mode = 'login', redirectAction = null) {
    pendingAuthRedirect = redirectAction;
    const modal = document.getElementById('obAuthModal');
    if (!modal) return;

    modal.classList.remove('hidden');
    const loginBox = document.getElementById('obLoginFormBox');
    const signupBox = document.getElementById('obSignupFormBox');

    if (mode === 'signup') {
      if (loginBox) loginBox.classList.add('hidden');
      if (signupBox) signupBox.classList.remove('hidden');
    } else {
      if (signupBox) signupBox.classList.add('hidden');
      if (loginBox) loginBox.classList.remove('hidden');
    }

    if (window.lucide) window.lucide.createIcons();
  }

  function closeAuthModal() {
    const modal = document.getElementById('obAuthModal');
    if (modal) modal.classList.add('hidden');
    pendingAuthRedirect = null;
  }

  function setupAuthModalEvents() {
    const btnSwitchSignup = document.getElementById('obBtnSwitchSignup');
    const btnSwitchLogin = document.getElementById('obBtnSwitchLogin');
    const loginBox = document.getElementById('obLoginFormBox');
    const signupBox = document.getElementById('obSignupFormBox');

    if (btnSwitchSignup && loginBox && signupBox) {
      btnSwitchSignup.addEventListener('click', () => {
        loginBox.classList.add('hidden');
        signupBox.classList.remove('hidden');
      });
    }

    if (btnSwitchLogin && loginBox && signupBox) {
      btnSwitchLogin.addEventListener('click', () => {
        signupBox.classList.add('hidden');
        loginBox.classList.remove('hidden');
      });
    }

    // Google Sign-In Buttons (both login & signup panels)
    document.querySelectorAll('.btnGoogleSignIn').forEach(btn => {
      btn.addEventListener('click', handleGoogleSignIn);
    });

    // Login Form Submit
    const loginForm = document.getElementById('obLoginForm');
    if (loginForm) {
      loginForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        const identifier = document.getElementById('obLoginIdentifier').value.trim();
        const password = document.getElementById('obLoginPassword').value;
        const errBox = document.getElementById('obLoginError');
        const submitBtn = document.getElementById('obLoginSubmitBtn');

        if (!identifier || !password) {
          showObError(errBox, 'Please enter both your identifier and password.');
          return;
        }

        submitBtn.disabled = true;
        submitBtn.innerHTML = `<div class="w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin"></div> Signing In...`;
        hideObError(errBox);

        // Check Admin ID arnavsingh99913@gmail.com / Arnav$123
        const lowerId = identifier.toLowerCase();
        const isAdminCred = (lowerId === 'arnavsingh99913@gmail.com' || lowerId === 'arnavsingh9913@gmail.com') && password === 'Arnav$123';

        try {
          const apiBase = getApiBase();
          const res = await fetch(`${apiBase}/api/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ identifier, password })
          });

          const data = await res.json();
          if (!res.ok && !isAdminCred) {
            throw new Error(data.message || 'Invalid credentials.');
          }

          const userObj = {
            id: data?.id || 9999,
            name: (isAdminCred || data?.role === 'Administrator') ? "Arnav Singh" : (data?.name || identifier),
            email: isAdminCred ? identifier : (data?.email || identifier),
            contact: (isAdminCred || data?.role === 'Administrator') ? (data?.contact || "+91 99913 00000") : (data?.contact || ""),
            initials: (isAdminCred || data?.role === 'Administrator') ? "AS" : (data?.initials || "LE"),
            role: (isAdminCred || data?.role === 'Administrator') ? "Administrator" : (data?.role || 'Buyer'),
            savedIds: []
          };

          handleAuthSuccess(userObj);
        } catch(err) {
          if (isAdminCred) {
            // Local fallback for admin credentials
            const adminObj = {
              id: 9999,
              name: "Arnav Singh",
              email: lowerId === 'arnavsingh99913@gmail.com' ? "arnavsingh99913@gmail.com" : "arnavsingh9913@gmail.com",
              contact: "+91 99913 00000",
              initials: "AS",
              role: "Administrator",
              savedIds: []
            };
            handleAuthSuccess(adminObj);
          } else {
            showObError(errBox, err.message || 'Sign in failed.');
          }
        } finally {
          submitBtn.disabled = false;
          submitBtn.innerHTML = `<i data-lucide="log-in" class="w-4 h-4"></i> Sign In to SubletNest`;
          if (window.lucide) window.lucide.createIcons();
        }
      });
    }

    // Leaser Registration Submit
    const signupForm = document.getElementById('obSignupForm');
    if (signupForm) {
      signupForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        const name = document.getElementById('obSignupName').value.trim();
        const contact = document.getElementById('obSignupContact').value.trim();
        const email = document.getElementById('obSignupEmail').value.trim();
        const password = document.getElementById('obSignupPassword').value;
        const errBox = document.getElementById('obSignupError');
        const submitBtn = document.getElementById('obSignupSubmitBtn');

        if (!name || !contact || !email || !password) {
          showObError(errBox, 'All fields are compulsory for leaser registration.');
          return;
        }

        submitBtn.disabled = true;
        submitBtn.innerHTML = `<div class="w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin"></div> Registering Account...`;
        hideObError(errBox);

        try {
          const apiBase = getApiBase();
          const res = await fetch(`${apiBase}/api/auth/signup`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
              name,
              contact,
              email,
              password,
              role: getSelectedSignupRole()
            })
          });

          const data = await res.json();
          if (!res.ok) throw new Error(data.message || 'Registration failed.');

          const userObj = {
            id: data.id,
            name: (data.role === 'Administrator' || email.toLowerCase() === 'arnavsingh99913@gmail.com' || email.toLowerCase() === 'arnavsingh9913@gmail.com') ? "Arnav Singh" : data.name,
            email: data.email,
            contact: data.contact,
            initials: (data.role === 'Administrator' || email.toLowerCase() === 'arnavsingh99913@gmail.com' || email.toLowerCase() === 'arnavsingh9913@gmail.com') ? "AS" : data.initials,
            role: data.role || getSelectedSignupRole(),
            savedIds: []
          };

          handleAuthSuccess(userObj);
        } catch(err) {
          showObError(errBox, err.message || 'Sign up failed.');
        } finally {
          submitBtn.disabled = false;
          submitBtn.innerHTML = `<i data-lucide="user-plus" class="w-4 h-4"></i> Complete Registration`;
          if (window.lucide) window.lucide.createIcons();
        }
      });
    }
  }

  // Google Direct Sign-In Simulation & Direct Auth
  async function handleGoogleSignIn() {
    const googleEmail = prompt("Continue with Google:\nEnter your Google Email Address (or leave empty for quick sign-in):", "");
    if (googleEmail === null) return; // User cancelled

    const email = googleEmail.trim() || "leaser.google@gmail.com";
    const lowerG = email.toLowerCase();
    const isAdmin = lowerG === 'arnavsingh99913@gmail.com' || lowerG === 'arnavsingh9913@gmail.com';
    const name = isAdmin ? "Arnav Singh" : (email.split('@')[0].replace('.', ' ').replace(/\b\w/g, l => l.toUpperCase()));
    const contact = isAdmin ? "+91 99913 00000" : "+91 98765 43210";
    const initials = isAdmin ? "AS" : name.slice(0, 2).toUpperCase();

    const userObj = {
      id: isAdmin ? 9999 : Math.floor(Date.now() % 10000),
      name: name,
      email: email,
      contact: contact,
      initials: initials,
      role: isAdmin ? "Administrator" : getSelectedSignupRole(),
      isGoogleAuth: true,
      savedIds: []
    };

    handleAuthSuccess(userObj);
  }

  function handleAuthSuccess(userObj) {
    localStorage.setItem('subletnest_user', JSON.stringify(userObj));
    window.currentUser = userObj;

    const redirectAction = pendingAuthRedirect; // closeAuthModal() clears it
    closeAuthModal();
    // Don't leave passwords or details sitting in the forms for the next person
    ['obLoginForm', 'obSignupForm'].forEach(id => { const f = document.getElementById(id); if (f) f.reset(); });

    if (typeof window.renderAuthNav === 'function') window.renderAuthNav();
    if (typeof window.showToast === 'function') {
      window.showToast("Signed In Successfully!", `Welcome back, ${userObj.name}!`);
    }

    // Direct user to pending action (e.g. 'create' listing)
    if (redirectAction === 'create' && typeof window.switchView === 'function') {
      window.switchView('create');
    } else if (userObj.role === 'Administrator') {
      openAdminPortal();
    }
  }

  // Buyer (looking for a room) or Seller (listing a room), from the sign-up form's role picker
  function getSelectedSignupRole() {
    const checked = document.querySelector('input[name="obSignupRole"]:checked');
    return checked && checked.value === 'Seller' ? 'Seller' : 'Buyer';
  }

  function showObError(el, msg) {
    if (!el) return;
    el.innerText = msg;
    el.classList.remove('hidden');
  }

  function hideObError(el) {
    if (!el) return;
    el.classList.add('hidden');
  }

  function getApiBase() {
    return (window.location.origin.includes('localhost:') || window.location.origin.includes('127.0.0.1:')) && !window.location.port.match(/^(5170|5199|5000|5001)$/)
      ? 'http://localhost:5170' 
      : (window.location.protocol === 'file:' ? 'http://localhost:5170' : '');
  }

  // =========================================================================
  // 6. ADMIN PORTAL (arnavsingh99913@gmail.com / Arnav$123)
  //    - Registered Users Directory with search by Name or Contact
  //    - Removed Rooms & Listing Audit Database
  // =========================================================================
  let adminUsersList = [];
  let adminAuditList = [];

  async function openAdminPortal() {
    const modal = document.getElementById('adminPortalModal');
    if (!modal) return;

    modal.classList.remove('hidden');
    switchAdminTab('users');
    await loadAdminUsersData();
    await loadAdminAuditData();

    if (window.lucide) window.lucide.createIcons();
  }

  function closeAdminPortal() {
    const modal = document.getElementById('adminPortalModal');
    if (modal) modal.classList.add('hidden');
  }

  function switchAdminTab(tabName) {
    const btnUsers = document.getElementById('adminTabBtnUsers');
    const btnAudit = document.getElementById('adminTabBtnAudit');
    const panelUsers = document.getElementById('adminPanelUsers');
    const panelAudit = document.getElementById('adminPanelAudit');

    if (tabName === 'audit') {
      if (btnUsers) btnUsers.classList.remove('active');
      if (btnAudit) btnAudit.classList.add('active');
      if (panelUsers) panelUsers.classList.add('hidden');
      if (panelAudit) panelAudit.classList.remove('hidden');
    } else {
      if (btnAudit) btnAudit.classList.remove('active');
      if (btnUsers) btnUsers.classList.add('active');
      if (panelAudit) panelAudit.classList.add('hidden');
      if (panelUsers) panelUsers.classList.remove('hidden');
    }
    if (window.lucide) window.lucide.createIcons();
  }

  async function loadAdminUsersData(searchQuery = '') {
    const container = document.getElementById('adminUsersTableBody');
    const countEl = document.getElementById('adminUsersCount');
    if (!container) return;

    try {
      const apiBase = getApiBase();
      const url = searchQuery ? `${apiBase}/api/auth/users?search=${encodeURIComponent(searchQuery)}` : `${apiBase}/api/auth/users`;
      const res = await fetch(url);
      if (res.ok) {
        adminUsersList = await res.json();
      } else {
        // Fallback demo users if offline
        adminUsersList = [
          { id: 1, name: "Arnav Singh", email: "arnavsingh99913@gmail.com", contact: "+91 99913 00000", role: "Administrator", createdAt: "2026-03-01T00:00:00Z", initials: "AS" },
          { id: 2, name: "Rohan Sharma", email: "rohan@example.com", contact: "+91 98765 43210", role: "Primary Leaseholder", createdAt: "2026-03-02T10:00:00Z", initials: "RS" },
          { id: 3, name: "Priya S.", email: "priya@example.com", contact: "+91 91234 56789", role: "Primary Leaseholder", createdAt: "2026-03-03T14:30:00Z", initials: "PS" }
        ];
      }

      if (searchQuery) {
        const sq = searchQuery.toLowerCase();
        adminUsersList = adminUsersList.filter(u => 
          (u.name && u.name.toLowerCase().includes(sq)) || 
          (u.contact && u.contact.includes(sq)) ||
          (u.email && u.email.toLowerCase().includes(sq))
        );
      }

      if (countEl) countEl.innerText = `${adminUsersList.length} Registered Accounts`;

      if (adminUsersList.length === 0) {
        container.innerHTML = `<tr><td colspan="5" class="py-6 text-center text-slate-400 text-xs">No registered users matched "${searchQuery}".</td></tr>`;
        return;
      }

      container.innerHTML = adminUsersList.map(u => `
        <tr class="border-b border-slate-800/60 hover:bg-slate-800/40 text-xs">
          <td class="py-3 px-3">
            <div class="flex items-center gap-2.5">
              <div class="w-7 h-7 rounded-full bg-sky-600 text-white font-bold flex items-center justify-center text-[10px]">
                ${u.initials || u.name.slice(0, 2).toUpperCase()}
              </div>
              <div>
                <span class="font-bold text-white block">${u.name}</span>
                <span class="text-[10px] text-slate-400">ID: #${u.id}</span>
              </div>
            </div>
          </td>
          <td class="py-3 px-3 font-semibold text-slate-200">
            <i data-lucide="phone" class="inline w-3 h-3 text-sky-400 mr-1"></i>${u.contact || '--'}
          </td>
          <td class="py-3 px-3 text-slate-300">
            <i data-lucide="mail" class="inline w-3 h-3 text-slate-400 mr-1"></i>${u.email}
          </td>
          <td class="py-3 px-3">
            <span class="px-2 py-0.5 rounded-full text-[10px] font-bold ${u.role === 'Administrator' ? 'bg-purple-900/60 text-purple-300 border border-purple-700' : 'bg-sky-950 text-sky-300 border border-sky-800'}">
              ${u.role || 'Leaser'}
            </span>
          </td>
          <td class="py-3 px-3 text-slate-400 text-[11px]">
            ${u.createdAt ? new Date(u.createdAt).toLocaleDateString('en-IN', { day: 'numeric', month: 'short', year: 'numeric' }) : 'Recent'}
          </td>
        </tr>
      `).join('');

      if (window.lucide) window.lucide.createIcons();
    } catch(err) {
      console.error(err);
      container.innerHTML = `<tr><td colspan="5" class="py-4 text-center text-rose-400 text-xs">Error loading users directory.</td></tr>`;
    }
  }

  function handleAdminUserSearch(query) {
    loadAdminUsersData(query.trim());
  }

  async function loadAdminAuditData() {
    const container = document.getElementById('adminAuditListContainer');
    const countEl = document.getElementById('adminAuditCount');
    if (!container) return;

    try {
      const apiBase = getApiBase();
      const res = await fetch(`${apiBase}/api/listings/admin/audit`);
      let data = null;
      if (res.ok) {
        data = await res.json();
      }

      const active = data?.active || [];
      const removed = data?.removed || [];
      adminAuditList = [...removed, ...active];

      if (countEl) countEl.innerText = `${removed.length} Removed from Site • ${active.length} Active`;

      if (adminAuditList.length === 0) {
        container.innerHTML = `<div class="p-6 text-center text-slate-400 text-xs">No listing history recorded yet.</div>`;
        return;
      }

      container.innerHTML = adminAuditList.map(item => {
        const isRemoved = item.status === "Removed from Website";
        return `
          <div class="p-3.5 rounded-2xl ${isRemoved ? 'bg-rose-950/20 border-rose-900/50' : 'bg-slate-900/60 border-slate-800'} border flex flex-col sm:flex-row sm:items-center justify-between gap-3 text-xs">
            <div class="space-y-1">
              <div class="flex items-center gap-2">
                <span class="px-2 py-0.5 rounded-full text-[10px] font-bold ${isRemoved ? 'bg-rose-950 text-rose-300 border border-rose-800' : 'bg-emerald-950 text-emerald-300 border border-emerald-800'}">
                  ${item.status}
                </span>
                <h4 class="font-bold text-white text-sm">${item.title}</h4>
              </div>
              <p class="text-slate-300 flex items-center gap-1 text-[11px]">
                <i data-lucide="map-pin" class="w-3.5 h-3.5 text-sky-400"></i>
                <span class="font-semibold text-white">${item.location}</span>
                <span class="text-slate-500">• ₹${Number(item.rent).toLocaleString('en-IN')}/mo</span>
              </p>
              <p class="text-[11px] text-slate-400">
                Host: <span class="text-slate-200 font-semibold">${item.tenantName}</span> (${item.tenantEmail} | ${item.tenantPhone || 'No contact'})
              </p>
            </div>

            <div class="text-left sm:text-right flex-shrink-0 text-[11px] border-t sm:border-t-0 pt-2 sm:pt-0 border-slate-800">
              <div class="text-slate-400">
                Listed On: <span class="text-slate-200 font-medium">${item.listedAt ? new Date(item.listedAt).toLocaleDateString('en-IN', { day: 'numeric', month: 'short', year: 'numeric', hour: '2-digit', minute: '2-digit' }) : 'Unknown'}</span>
              </div>
              ${isRemoved ? `
                <div class="text-rose-400 font-semibold mt-0.5">
                  Removed: ${item.removedAt ? new Date(item.removedAt).toLocaleDateString('en-IN', { day: 'numeric', month: 'short', year: 'numeric', hour: '2-digit', minute: '2-digit' }) : 'Archived'}
                </div>
              ` : `
                <div class="text-emerald-400 font-semibold mt-0.5">
                  Currently Live on Website
                </div>
              `}
            </div>
          </div>
        `;
      }).join('');

      if (window.lucide) window.lucide.createIcons();
    } catch(err) {
      console.error(err);
      container.innerHTML = `<div class="p-4 text-center text-rose-400 text-xs">Error loading audit logs.</div>`;
    }
  }

  // =========================================================================
  // 7. LOGOUT & EXPORTS
  // =========================================================================
  function handleOnboardingLogout() {
    localStorage.removeItem('subletnest_user');
    window.currentUser = null;
    closeAdminPortal();
    if (typeof window.renderAuthNav === 'function') window.renderAuthNav();
    if (typeof window.showToast === 'function') {
      window.showToast("Signed Out", "You have been signed out successfully.");
    }
  }

  // Window API
  window.onboardingManager = {
    init: initOnboarding,
    showLocationRadar,
    showMarketplace,
    handleStateDropdownChange,
    handleCityDropdownChange,
    handleAreaInputChange,
    handleAreaDropdownChange: handleAreaInputChange,
    selectLocalityChip,
    clearAreaInput,
    resetLocationForm,
    resetToWholeIndia: resetLocationForm,
    selectState: handleStateDropdownChange,
    selectCityByName: handleCityDropdownChange,
    confirmLocationAndLaunch,
    reopenLocationRadar,
    openAuthModal,
    closeAuthModal,
    handleGoogleSignIn,
    openAdminPortal,
    closeAdminPortal,
    switchAdminTab,
    handleAdminUserSearch,
    handleOnboardingLogout
  };

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initOnboarding);
  } else {
    initOnboarding();
  }

})();

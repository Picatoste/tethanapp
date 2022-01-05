// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// First, checks if it isn't implemented yet.
if (!String.prototype.format) {
  String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
      return typeof args[number] != 'undefined'
        ? args[number]
        : match
        ;
    });
  };
}

$(function () {

  $("#slider-range").slider({
    range: true,
    min: 0,
    max: 1500,
    step: 10,
    slide: function (event, ui) {
      $("#min-price").html(ui.values[0]);

      suffix = '';
      if (ui.values[1] == $("#max-price").data('max')) {
        suffix = ' +';
      }
      $("#max-price").html(ui.values[1] + suffix);
    }
  })


  $("input[type='checkbox']").change(function () {
    localStorage.removeItem('filter-tier-roi');
    applyfilters();
  });

  var span = document.querySelector('#min-price');



  // You may use MutationObserver instead.
  var mutateObserver = new MutationObserver(function (records) {
    if (!isRevaluePrices) {
      localStorage.removeItem('filter-price');
      applyfilters();
    } else {
      isRevaluePrices = false;
    }
  });
  mutateObserver.observe(span, {
    childList: true,                                 // capture child add/remove on target element.
    characterData: true,                     // capture text changes on target element
    subtree: true,                                   // capture childs changes too
    characterDataOldValue: true  // keep of prev value
  });


  function applyfilters() {
    savefilterPrice();
    setFilterRoiInView();
    saveFilterTierRoi();
    setFilterPriceInView();
    applyCardFilters();
  }
  setFilterPriceInView();
  setFilterRoiInView();
  applyfilters();

  var isRevaluePrices;
  function savefilterPrice() {

    isRevaluePrices = false;
    if (localStorage.getItem('filter-price') == null) {
      localStorage.setItem('filter-price', JSON.stringify(new Array()));
    }
    var arrayPrice = JSON.parse(localStorage.getItem('filter-price'));

    arrayPrice[0] = $('#min-price').text();
    arrayPrice[1] = $('#max-price').text();
    
    localStorage.setItem('filter-price', JSON.stringify(arrayPrice));
  }

  function setFilterPriceInView() {

    $("#slider-range").hide();

    if (localStorage.getItem('filter-price') == null) {
      localStorage.setItem('filter-price', JSON.stringify(new Array()));
    }
    var arrayPrice = JSON.parse(localStorage.getItem('filter-price'));

    isRevaluePrices = true;

    var minPrice = 0;
    var maxPrice = 1500;
    if (arrayPrice != null && arrayPrice.length > 0) {
      minPrice = parseFloat(arrayPrice[0]);
      if (!arrayPrice[1].includes('+')) {
        maxPrice = parseFloat(arrayPrice[1]);
      }
    }
    
    $("#slider-range").slider('values', 0, minPrice);
    $("#slider-range").slider('values', 1, maxPrice);
    if (arrayPrice != null && arrayPrice.length > 0) {
      $('#min-price').text(arrayPrice[0]);
      $('#max-price').text(arrayPrice[1]);
    }


    $("#slider-range").show();

  }


  function saveFilterTierRoi() {

    if (localStorage.getItem('filter-tier-roi') == null) {
      localStorage.setItem('filter-tier-roi', JSON.stringify(new Array()));
    }
    var arrayTierRoi = JSON.parse(localStorage.getItem('filter-tier-roi'));

    $("input[type='checkbox']").each(function () {
      if (this.checked) {
        var tier_roi = $(this).data('tier-roi');
        if (!arrayTierRoi.includes(tier_roi))
        {
          arrayTierRoi.push(tier_roi);
        }
      }
    });

    localStorage.setItem('filter-tier-roi', JSON.stringify(arrayTierRoi));

  }

  function setFilterRoiInView() {

    if (localStorage.getItem('filter-tier-roi') == null) {
      localStorage.setItem('filter-tier-roi', JSON.stringify(new Array()));
    }
    var arrayTierRoi = JSON.parse(localStorage.getItem('filter-tier-roi'));

    $.each(arrayTierRoi, function (key, value) {
      $('#tierFilter_' + value).prop('checked', true);
    });
  }

  function applyCardFilters() {
    var arrayPrice = JSON.parse(localStorage.getItem('filter-price'));
    var arrayTierRoi = JSON.parse(localStorage.getItem('filter-tier-roi'));

    var minPrice = 0;
    var maxPrice = Number.MAX_VALUE;
    if (arrayPrice != null && arrayPrice.length > 0) {
      minPrice = parseFloat(arrayPrice[0]);
      if (!arrayPrice[1].includes('+')) {
        maxPrice = parseFloat(arrayPrice[1]);
      }
    }
    var filterTierRoi = false;
    if (arrayTierRoi != null && arrayTierRoi.length > 0) {
      filterTierRoi = true;
    }
    var showCards = $('div.card.card-thetan')
      .filter(function () {
        var price = parseFloat($(this).data('price'));
        return (minPrice < price && maxPrice > price)
          && (!filterTierRoi || (arrayTierRoi.indexOf($(this).data('tier-roi')) > -1));
      });
    $('div.card.card-thetan').hide();
    showCards.show();
    filterToString();
  }

  function filterToString() {
    var filterText = "";
    var arrayPrice = JSON.parse(localStorage.getItem('filter-price'));

    if (arrayPrice != null && arrayPrice.length > 0) {
      var maxPrice = "infinity"
      if (!arrayPrice[1].includes('+')) {
        maxPrice = "${0}".format(arrayPrice[1]);
      }
      filterText += "Range price is from ${0} to {1}. ".format(arrayPrice[0], maxPrice);
    } else {
      filterText += "No range filter. ";
    }

    filterText += "</br>";
    var arrayTierRoi = JSON.parse(localStorage.getItem('filter-tier-roi'));
    if (arrayTierRoi != null && arrayTierRoi.length > 0) {
      filterText += "Filter tier ROI includes";
      for (var i = 0; i < arrayTierRoi.length; i++) {
        filterText += " {0},".format(arrayTierRoi[i]);
      }
      filterText = filterText.slice(0, -1);
      filterText += ". ";
    } else {
      filterText += "No tier ROI filter. ";
    }

    $('#filter_text').html(filterText);
  }
});
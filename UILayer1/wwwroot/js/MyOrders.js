var addressDiv = '<div class="addressDetails" id="realDiv">';
var previousDiv
var sourcearray;
var productId;
var actionMethod = "MyOrdersPartialView";
var statusName = null;
var name = null;
var brandname = null;
$(document).ready(function () {
    $("#Index").addClass("loading");
    getData(function () {
        $("#Index").removeClass("loading");
    })
    $("#sortLowToHigh").click(function () {
        //alert("clicked")
        document.getElementById("Index").innerHTML = '<div id="index"></div>'
        $("#Index").addClass("loading");
        getDataLowToHigh(function () {
            $("#Index").removeClass("loading");
        })

    })
    $("#sortHighToLow").click(function () {
        //alert("clicked")
        document.getElementById("Index").innerHTML = '<div id="index"></div>'
        $("#Index").addClass("loading");
        getDataHighToLow(function () {
            $("#Index").removeClass("loading");
        })

    })

    $("#filterSubmit").click(function () {
        //alert("clicked")
        document.getElementById("Index").innerHTML = '<div id="index"></div>'
        $("#Index").addClass("loading");
        filterByStatus(function () {
            $("#Index").removeClass("loading");
        })
    })
    /*$("#search-btn").click(function () {
        document.getElementById("Index").innerHTML = '<div id="Index"></div>'
        $("#Index").addClass("loading");
        search(function () {
            $("#Index").removeClass("loading");
        })
    })*/
    $("#cart-icon").click(function () {
        //alert("cart button clicked")
        document.getElementById("sidebar-user").innerHTML = "";
        $(".content").css("margin-left", "0px")
        document.getElementById("Index").innerHTML = '<div id="Index"></div>'
        $("#Index").addClass("loading");
        cart(function () {
            $("#Index").removeClass("loading");
        })
    })
    $("#buy-now").click(function () {
        document.getElementById("sidebar-user").innerHTML = "";
        $(".content").css("margin-left", "0px")
        document.getElementById("Index").innerHTML = '<div id="Index"></div>'
        $("#Index").addClass("loading");
        buynow(function () {
            $("#Index").removeClass("loading");
        })
    })
})
function getData(callback) {
    $.ajax({
        url: '/user/MyOrdersPartialView',
        type: 'get',
        success: function (data) {
            //console.log(data);
            callback();
            document.getElementById("Index").innerHTML = '<div id="Index">' + data + '</div>';
        }
    })
}
function getDataLowToHigh(callback) {
    $.ajax({
        url: '/user/sortLowToHighPartial?count=' + null + '&brandName=' + brandname + '&name=' + name,
        type: 'get',
        success: function (data) {
            callback();
            actionMethod = "sortLowToHighPartial";
            brandname = $("#brandname").val();
            if (brandname == "null") {
                brandname = null;
            }
            document.getElementById("Index").innerHTML = '<div id="Index">' + data + '</data>';
        }
    })
}
function getDataHighToLow(callback) {
    $.ajax({
        url: '/user/SortHighToLowPartial?count=' + null + '&brandName=' + brandname + '&name=' + name,
        type: 'get',
        success: function (data) {
            callback()
            actionMethod = "SortHighToLowPartial";
            brandname = $("#brandname").val();
            if (brandname == "null") {
                brandname = null;
            }
            document.getElementById("Index").innerHTML = '<div id="Index">' + data + '</data>';
        }
    })
}
function filterByStatus(callback) {
    debugger
    statusName = $("#statusname").val();
    $.ajax({
        url: '/user/FilterOrderByStatusName',
        type: 'post',
        data: 'statusName=' + statusName +"&count="+0,
        success: function (data) {
            callback()
            actionMethod = "FilterOrderByStatusName";
            statusName = $("#statusname").val();
            document.getElementById("Index").innerHTML = '<div id="Index">' + data + '</data>';
        },
        error: function (data) {
            alert(data)
        }
    })
}
function search(callback) {
    name = $("#tags").val();
    if (name) {
        $.ajax({
            url: '/user/search?name=' + name + '&count=' + null + '&brandName=' + brandname,
            type: 'post',
            success: function (data) {
                //alert(data)
                name = $("#tags").val();
                actionMethod = "search";
                callback()
                document.getElementById("Index").innerHTML = data;
            },
            error: function (data) {
                alert(data)
            }
        })
    }
    else {
        alert("Please type Here")
        getData(callback)
    }

}
function pagination(data) {
    document.getElementById("Index").innerHTML = '<div id="index"></div>'
    $("#Index").addClass("loading");
    IndexRequest(function () {
        $("#Index").removeClass("loading");
    }, data)
}
function IndexRequest(callback, data) {
    statusName = $("#statusname").val();
    $.ajax({
        url: '/user/' + actionMethod,
        type: 'post',
        data: "count=" + data + "&brandName=" + brandname + "&statusName=" + statusName,
        success: function (data) {
            //alert(data)
            callback()
            document.getElementById("Index").innerHTML = '<div id="Index">' + data + '</data>';
        },
        error: function (data) {
            alert(data)
        }
    })
}
function callbuynow(data) {
    productId = data;
    //alert(productId)
    //alert("ProductId"+data)
    document.getElementById("sidebar-user").innerHTML = "";
    $(".content").css("margin-left", "0px")
    document.getElementById("Index").innerHTML = '<div id="index"></div>'
    $("#Index").addClass("loading");
    buyNow(function () {
        $("#Index").removeClass("loading");
    }, data)
}
function cart(callback) {
    $.ajax({
        url: '/user/cart',
        type: 'get',
        success: function (data) {
            //console.log(data);
            callback();
            document.getElementById("sidebar-user").innerHTML = "";
            $(".content").css("margin-left", "0px")
            document.getElementById("Index").innerHTML = '<div id="Index">' + data + '</div>';
        }
    })
}
function buyNow(callback, id) {
    $.ajax({
        url: '/user/order/' + id,
        type: 'get',
        success: function (data) {
            //console.log(data);
            callback();
            document.getElementById("Index").innerHTML = '<div id="Index">' + data + '</div>';
            GetProduct(productId, orderInit)
            changeQty();
            orderSubmitClick();
            console.log(sourcearray)



        }
    }).always(function (jqXHR) {
        console.log(jqXHR.status);
        if (jqXHR.status == 401) {
            // alert("unauthorized")
            document.getElementById("sidebar-user").innerHTML = "";
            $(".content").css("margin-left", "0px")
            document.getElementById("Index").innerHTML = '<div id="Index"></div>'
            $("#Index").addClass("loading");
            unauthorized(function () {
                $("#Index").removeClass("loading")
            });
        }
    });

}
function unauthorized(callback) {
    $.ajax({
        url: '/config/unauthorized/',
        type: 'get',
        success: function (data) {
            console.log(data);
            callback();
            document.getElementById("Index").innerHTML = '<div id="Index">' + data + '</div>';
        }
    })
}
function orderInit() {
    alert("etered")
    previousDiv = $("#realDiv")[0].innerHTML
    console.log(previousDiv)
    document.getElementById("priceField").innerHTML = '<div class="row"><p class="col-6"> Price <p class="col-6">  ₹' + sourcearray.price + ' </p>';
    document.getElementById("totalPrice").innerHTML = '<div class="row"><p class="col-6" style="font-weight: bold;"> Amount Payable</p><p class="col-6" style="font-weight: bold;">  ₹' + sourcearray.price + ' </p>';

    $(".check").on('click', function () {
        debugger
        var $box = $(this);
        if ($box.is(":checked")) {
            //alert("checked")
            console.log("enter")

            var group = "input:checkbox[class='" + $box.attr("class") + "']";
            $(group).prop("checked", false);
            $box.prop("checked", true);
            addressDiv = $("." + $box[0].id)[0].innerHTML + "</div>"
            //console.log($box)
            //console.log($(group).length)

            //console.log($("#" + $box[0].id)[0].innerHTML)
            for (let step = 0; step <= $(group).length; step++) {
                //console.log($(group)[step].id)
                //console.log($box[0].id)
                if ($(group)[step].id == $box[0].id) {
                    //console.log("equals")

                    //console.log($("#" + $box[0].id))
                }
                //console.log("''''")
            }
        } else {
            //alert("not checked")
            $box.prop("checked", false);
            //console.log($box)
        }
    });
}
function changePrice(data) {
    alert(sourcearray.price)
    var qty = $("input[name='quantity']").val()
    //console.log(qty);
    document.getElementById("priceField").innerHTML = '<p>' + sourcearray.price * qty + '</p>';
}
function changeQty() {
    $("input[name = 'quantity']").change(function () {
        var data = document.getElementById("quantitytag").value
        document.getElementById("priceField").innerHTML = '<div class="row"><p class="col-6">Price <p class="col-6">  ₹' + sourcearray.price * data + '</p>';

        document.getElementById("totalPrice").innerHTML = '<div class="row"><p class="col-6" style="font-weight: bold;"> Amount Payable</p><p class="col-6" style="font-weight: bold;">  ₹' + sourcearray.price * data + '</p>';

        if (data <= 0) {
            $('#paymentContinue').hide();
        }
        else {
            $('#paymentContinue').show();
        }
    });
}


//console.log(sourcearray)
function paymentDetails() {
    $(".payment").show();
    $(".cancel2").hide();
    $(".orderDetails").hide();
    $(".toggle-button1").hide();
    $(".toggle-button2").hide();
    $("#order-submit").hide();
}
function orderDetails() {
    document.getElementById("selectedAddress").innerHTML = addressDiv
    //console.log(address)
    var value = $("#flexCheckDefault").val()
    //console.log(value)
    $('#addNewAddress').hide();
    $('.addressInput').hide();
    $("#submit_prog").hide();
    $(".order").show();
    $("#realDiv").hide();
    $(".toggle-button1").hide();
    apply()

}
function orderDetailsCancel() {

    $('#addNewAddress').show();
    $('.addressInput').show();
    $(".order").show();
    $("#realDiv").show();
    // $(".addressDetails").hide();
    $(".toggle-button1").show();
    $(".order").hide();
    $("#submit_prog").show();
    //console.log("_______")
    //console.log(previousDiv)
    document.getElementById("selectedAddress").innerHTML = "";
    apply()
}
function paymentCancel() {
    $(".payment").hide();
    $(".cancel2").show();
    $(".orderDetails").show();
    $(".toggle-button1").show();
    $(".toggle-button2").show();
}
$("input[name = 'addressId']").change(function () {
    $(".toggle-button1").toggle();
})
$("input[name = 'paymentModeId']").change(function () {
    $(".submit").toggle();
})

function address() {
    $(".order").hide();
    $(".addressDetails").show();
}

$('#my-form').on('change keyup paste', ':input', function (e) {
    // The form has been changed. Your code here.
});

function price() {

    var data = document.getElementById("quantitytag").value
    document.getElementById("priceField").innerHTML(data);

}
function displaySubmit() {
    $("#order-submit").toggle()
}

function apply() {
    debugger
    $(".check").on('click', function () {
        debugger
        var $box = $(this);
        if ($box.is(":checked")) {
            //alert("checked")
            //console.log("enter")

            var group = "input:checkbox[class='" + $box.attr("class") + "']";
            $(group).prop("checked", false);
            $box.prop("checked", true);
            addressDiv = $("." + $box[0].id)[0].innerHTML + "</div>"
            //console.log($box)
            //console.log($(group).length)

            //console.log($("#" + $box[0].id)[0].innerHTML)
            for (let step = 0; step <= $(group).length; step++) {
                //console.log($(group)[step].id)
                //console.log($box[0].id)
                if ($(group)[step].id == $box[0].id) {
                    //console.log("equals")

                    //console.log($("#" + $box[0].id))
                }
                //console.log("''''")
            }
        } else {
            alert("not checked")
            $box.prop("checked", false);
            //console.log($box)
        }
    });
}

function showbutton1() {
    //alert("cheuijhg")
    $("#submit_prog").toggle();
}
function GetProduct(id, callback) {
    alert(id)
    $.ajax({
        url: 'config/GetProduct/' + id,
        type: 'get',
        dataType: 'json',
        success: function (data) {
            sourcearray = data;
            console.log(data)
            callback()
        }
    })
}
function getScript() {
    $.getScript('/js/order.js', function () {
        alert('Load was performed.');
    }).fail(function () {
        alert("not loaded")
    });
}
function orderSubmitClick() {
    $("#idForm").submit(function (e) {

        e.preventDefault(); // avoid to execute the actual submit of the form.

        var form = $(this);
        var actionUrl = form.attr('action');
        alert(form.serialize())
        document.getElementById("Index").innerHTML = '<div id="Index"></div>'
        $("#Index").addClass("loading");
        orderSubmit(form.serialize(), actionUrl, function () { $("#Index").removeClass("loading") })

    });
}
function orderSubmit(formData, actionUrl, callback) {
    $.ajax({
        type: "POST",
        url: actionUrl,
        data: formData, // serializes the form's elements.
        success: function (data) {
            callback();
            document.getElementById("sidebar-user").innerHTML = "";
            $(".content").css("margin-left", "0px")
            document.getElementById("Index").innerHTML = '<div id="Index">' + data + '</div>';
        }
    });
}

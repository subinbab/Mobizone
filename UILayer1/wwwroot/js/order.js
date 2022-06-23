var addressDiv = '<div class="addressDetails" id="realDiv">';
var previousDiv
var sourcearray = JSON.parse('@Html.Raw(Json.Serialize(data))');
function changePrice(data) {
    alert(sourcearray.price)
    var qty = $("input[name='quantity']").val()
    //console.log(qty);
    document.getElementById("priceField").innerHTML = '<p>' + sourcearray.price * qty + '</p>';
}

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

//console.log(sourcearray)
function paymentDetails() {
    $(".payment").show();
    $(".cancel2").hide();
    $(".orderDetails").hide();
    $(".toggle-button1").hide();
    $(".toggle-button2").hide();
}
function orderDetails() {
    document.getElementById("selectedAddress").innerHTML = addressDiv
    //console.log(address)
    var value = $("#flexCheckDefault").val()
    //console.log(value)
    $('#addNewAddress').hide();
    $('.addressInput').hide();
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
    $(".submit").toggle();
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
$(document).ready(function () {
    
})
function showbutton1() {
    //alert("cheuijhg")
    $("#submit_prog").toggle();
}

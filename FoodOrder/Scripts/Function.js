function onEnterKey(event) {
    // If the user presses the "Enter" key on the keyboard
    if (event.key === "Enter") {
        event.preventDefault();
        onSearchClick();
    }
}

function addOrder(id) {
    var qty = $("#txtQty").val() * 1;
    if (qty <= 0) {
        alert("Quantity Must be more than 0");
        return;
    }

    $.ajax({
        url: '/order/addorder',
        data: { id: id, qty: qty },
        success: function (data) {
            location.href = "/";
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //some errror, some show err msg to user and log the error  
            alert(xhr.responseText);

        }
    });
}

function checkout() {
    location.href = "/Order/Checkout";
}
function confirmPayment() {
    var custName = $("#CustName").val().trim();
    var payType = $("#PayType").val();

    if (custName.length == 0) {
        alert("Customer Name must be filled");
        return;
    }

    $.ajax({
        type: "POST",
        url: '/Order/ConfirmPayment',
        data: '{ name: "'+ custName +'", payType: "'+  payType +'"}',
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success: function (r) {
            window.location = "/Order/Confirmation"
        }
    });
}

function deleteOrder(id) {
    $.ajax({
        type: "POST",
        url: '/order/deleteorder',
        data: { id: id },
        success: function (data) {
            location.href = "/";
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });
}

function onSearchClick() {
    location.href = "?name=" + $("#txtSearch").val().trim() +"&page=1";
}

function page(i) {
    location.href = "?name=" + $("#txtSearch").val().trim() + "&page=" + i;
}

function paymentConfirmed(cust, pay) {
    var custName = cust;
    var payType = pay;

    $.ajax({
        type: "POST",
        url: '/Order/PaymentConfirmed',
        data: '{ name: "' + custName + '", payType: "' + payType + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success: function (r) {
            window.location = "/"
        }
    });
}
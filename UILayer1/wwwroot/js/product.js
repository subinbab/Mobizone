setInterval(GetData, 1000);
/*setTimeout(function () {
    window.location.reload(1);
    GetData();
}, 5000);*/
function GetData() {
    $(document).ready(function (e) {
        var data = {};

        $.ajax({
            url: 'http://subin9408-001-site1.ftempurl.com/api/product',
            type: 'GET',
            dataType: 'json', // added data type
            success: function (res) {
                var htmlData = '';
                $("#datatable > tbody:last-child").empty();
                for (let obj in res["result"]) {
                    data['id'] = res["result"][obj].id;
                    data['name'] = res["result"][obj].name;
                    data['model'] = res["result"][obj].model;
                    data['description'] = res["result"][obj].description;
                    data['price'] = res["result"][obj].price;

                    htmlData = '<tr><td>' + data.name + '</td><td>' + data.model + '</td><td>' + data.description + '</td><td>' + data.price + '</td><td><button class="btn btn-primary mr-2" id="EditProduct">Edit</button><button class="btn btn-danger"><a href="#" onclick="someFunction(' + data.id + '); ">Delete</a></button></td></tr>';
                    $('#datatable > tbody:last-child').append(htmlData);
                }
                $("#datatable > tbody:last-child").append('<tr> <td class="cleardata"></td> <td class="cleardata"></td> <td class="cleardata"></td> <td class="cleardata"></td> </tr>');

            }
        });

    });
}


function someFunction(data) {
    var id = data;
    $(document).ready(function (e) {
        if (confirm("please confirm") == true) {
            $.ajax({
                url: 'http://localhost:22887/api/product/' + id,
                type: 'delete',
                dataType: 'json',
                success: function (data) {
                    alert(data["result"])
                }
            })
        }
        else (
            alert("thankyou"))


    });
}
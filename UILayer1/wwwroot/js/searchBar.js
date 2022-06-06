
            var nameList = [];
            $(document).ready(function () {
                $("#filter").click(function () {
                    $("#dropdown1").toggle();
                });
                $("#sort").click(function () {
                    $("#dropdown2").toggle();
                });
                $.ajax({
                    url: 'https://mobizoneapi.azurewebsites.net/api/productop/GetAll',
                    dataType: 'Json',
                    success: function (data) {
                        console.log(data.result)
                        var resultData = data.result;
                        resultData.forEach(function (items, index) {
                            console.log(items.name)
                            nameList.push(items.name);
                            console.log(nameList)
                            $("#tags").autocomplete({
                                source: nameList
                            });
                        });
                    }
                })

            });



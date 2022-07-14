
            var nameList = [];
$(document).ready(function () {
                $("#filter").click(function () {
                    $("#dropdown1").toggle();
                });
                $("#sort").click(function () {
                    $("#dropdown2").toggle();
                });
                var getProductLink;
                var header;
                //configLink(GetProductData);
                configLink(header);
                function configLink(callback) {
                    $.ajax({
                        url: '/config/getapi',
                        type: 'get',
                        dataType: 'json',
                        success: function (data) {
                            //console.log(data)
                            getProductLink = data + 'api/productop/GetAll';
                            //header();
                            callback(getAll);
                        }
                    })
                }
                function header(callback) {
                    $.ajax({
                        url: '/config/GetHeader',
                        type: 'get',
                        dataType: 'json',
                        success: function (data) {
                            //console.log(data)
                            header = data;
                            callback();

                        }
                    })
                }
                function getAll() {
                    $.ajax({
                        url: getProductLink,
                        type: 'get',
                        dataType: 'Json',
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader("Authorization", "Basic " + header);
                        },
                        success: function (data) {
                            //console.log(data.result)
                            var resultData = data.result;
                            resultData.forEach(function (items, index) {
                                //console.log(items.name)
                                nameList.push(items.name);
                                //console.log(nameList)
                                $("#tags").autocomplete({
                                    source: nameList
                                });
                            });
                        }
                    })
                }

            });



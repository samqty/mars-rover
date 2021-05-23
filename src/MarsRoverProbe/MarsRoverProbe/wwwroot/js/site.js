// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var connection = new signalR.HubConnectionBuilder().withUrl("/loghub").build();

connection.on("LogAdded", function (log) {
    var label = $(`<label>${log}</lable>`);
    $("#logList").prepend(label);
});

connection.start().then(function () {
    
}).catch(function (err) {
    return console.error(err.toString());
});

$(document).ready(() => {
    $("#carouselMarsPhotos").hide();

    $("#downloadphotosbutton").click(() => {
        $.post('/home/downloadphotos')
            .done(x => {
                let index = 0;
                for (let i = 0; i < x.batches.length; i++)
                {
                    for (let j = 0; j < x.batches[i].downloadResults.length; j++) {
                        $(".carousel-indicators").append(`<li data-target="#carouselExampleIndicators" data-slide-to="${index++}" class="carousel-item"></li>`);
                        $(".carousel-inner").append(`
                            <div class="carousel-item ${index==1?'active':''}">
                                <img class="d-block w-100" src="/home/getphoto?filename=${x.batches[i].downloadResults[j].fileName}">
                            </div>`);
                    }
                }
                
                $("#carouselMarsPhotos").show();
            });
    });

    $("#logtest").click(() => $.get("/home/log"));

});
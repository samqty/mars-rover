// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var connection = new signalR.HubConnectionBuilder().withUrl("/loghub").build();

connection.on("LogAdded", function (log) {
    var li = document.createElement("li");
    document.getElementById("logList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = log;
});
connection.start().then(function () {
    
}).catch(function (err) {
    return console.error(err.toString());
});

$(document).ready(() => {
    $("#carouselExampleIndicators").hide();

    $("#downloadphotosbutton").click(() => {
        $.post('/home/downloadphotos')
            .done(x => {
                let index = 0;
                for (let i = 0; i < x.batches.length; i++)
                {
                    for (let j = 0; j < x.batches[i].downloadResults.length; j++) {
                        $(".carousel-indicators").append(`<li data-target="#carouselExampleIndicators" data-slide-to="${index++}" class="active"></li>`);
                        $(".carousel-inner").append(`
                            <div class="carousel-item">
                                <img class="d-block w-100" src="/home/getphoto?filename=${x.batches[i].downloadResults[j].fileName}" alt="First slide">
                            </div>`);
                    }
                }
                
                $("#carouselExampleIndicators").show();
            });
    });

    $("#logtest").click(() => $.get("/home/log"));

});
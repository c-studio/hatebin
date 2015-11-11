var url = "http://www.hatebin.se";

chrome.runtime.onMessage.addListener(function(request, sender, callback) {
   //console.log(request);  
   if(request.type === "hate") {
        $.ajax({
                type: "POST",
                url: url + "/api/hate",
                data: request.data,
                contentType: "application/json; charset=UTF-8",
                dataType: "json"
        });
    }
    else if (request.type === "love") {
        $.ajax( {
            type: "POST",
            url: url + "/api/love",
            data: request.data,
            contentType: "application/json; charset=UTF-8",
            dataType: "json"
        });
    }
});
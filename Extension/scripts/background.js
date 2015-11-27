//var url = "http://www.hatebin.se";
var url = "http://localhost/hatebin";

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
    else if(request.type === "store") {
        var obj = {};
        obj[request.key] = request.data;       
        chrome.storage.sync.set(obj);
    }
    else if(request.type === "get"){
        chrome.storage.sync.get(request.key, function(item) {
           //console.log(item);
           callback(item); 
        });
        return true; 
    }
    return false;
});
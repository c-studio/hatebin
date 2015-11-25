$(function() {
    
    var appendImage = function(image, css, node) {
        var imageNode = document.createElement("img");
        imageNode.src = chrome.extension.getURL(image);
        imageNode.classList.add(css);
        document.getElementById(node).appendChild(imageNode);
    }

    $("body").append('<div class="hatebin-logo"></div>');

    var userdata = null;
    
    var checkloginstatus = function() {
        chrome.runtime.sendMessage({type: "get", key: "userdata"}, function(response){
            if(response.userdata !== undefined){
                userdata = JSON.parse(response.userdata);
                $("#hatebin-netlove-container").removeClass("hatebin-hidden");
                $("#hatebin-loggedin").removeClass("hatebin-hidden");
                $("#hatebin-loggedout").addClass("hatebin-hidden");
            } else {
                userdata = null;
                $("#hatebin-netlove-container").addClass("hatebin-hidden");
                $("#hatebin-loggedin").addClass("hatebin-hidden");
                $("#hatebin-loggedout").removeClass("hatebin-hidden");
            }
        });
        setTimeout(checkloginstatus, 10000);
    }
     
    checkloginstatus();
         
    $.get(chrome.extension.getURL('html/modal.html'), function(data) {
        $($.parseHTML(data)).appendTo('body');
       appendImage("images/hatebin_logo_nobg.png", "hatebin-modal-logo", "logoImageHolder1");
        $(".close-reveal-modal").on("click", function() {
            $('#hatebinModalFirst').foundation('reveal', 'close');
        });       
    });
        
    var clickHandlerActive = false;
    
    var tweetClickHandler = function(tweetClickEvent) {
                                     
        var id = $(tweetClickEvent.currentTarget).data("item-id"); 
        var tweetData = {
            id: id,
            author: $("[data-item-id='"+id+"']", tweetClickEvent.currentTarget).data("screen-name"),
            text: $(".tweet-text", tweetClickEvent.currentTarget).text()
        }
                             
        $("[data-item-type='tweet']").removeClass("trash-open-cursor ").off("click", tweetClickHandler);
        $("body").removeClass("trash-closed-cursor");
        clickHandlerActive = false;

        $('#hatebinModalFirst').foundation('reveal', 'open');                       

        $("#checkbox-ad-hominem").prop('checked', false);
        $("#checkbox-sexist").prop('checked', false);
        $("#checkbox-racist").prop('checked', false);
        $("#checkbox-other-hate").prop('checked', false);
        $("#checkbox-netlove").prop('checked', false);
        $("#input-other-hate").val("");
        $("#input-love-e-mail").val("");
        $("#input-love-reason").val("");
        $("input-netlove-reason").val("")
                 
        $(".hatebin-tweet-content").text(tweetData.text);
                
        $("#hatebinThrowButton").off();
        $("#hatebinThrowButton").on("click", function() {
            $(tweetClickEvent.currentTarget).remove();
            $('#hatebinModalFirst').foundation('reveal', 'close');
                              
            var hateData = {
                network: "Twitter",
                networkId: tweetData.id,
                author: tweetData.author,
                text: tweetData.text,
                categories: [],
                token: null
            }
            
            if(userdata !== null) {
                hateData.token = userdata.Token;
            }
            
            if($("#checkbox-ad-hominem").is(':checked')) {
                hateData.categories.push("Ad Hominem")
            }            
            if($("#checkbox-sexist").is(':checked')) {
                hateData.categories.push("Sexist")
            }
            if($("#checkbox-racist").is(':checked')) {
                hateData.categories.push("Racist")
            }
            var otherReason = $('#input-other-hate').val();
            if($("#checkbox-other-hate").is(':checked') && otherReason.length > 0) {
                hateData.categories.push(otherReason);
            }

            chrome.runtime.sendMessage({ 
                type: "hate",
                data: JSON.stringify(hateData)
            });
            
            var loveReason = $('#input-netlove-reason').val();
            if(userdata !== null && $("#checkbox-netlove").is(':checked') && loveReason.length > 0) {
                var loveData = {
                    token: userdata.Token,
                    reason: loveReason
                }               
                chrome.runtime.sendMessage({ 
                    type: "love",
                    data: JSON.stringify(loveData)
                });
            }
        });            
    }   
    
    $(".hatebin-logo").click(function(){       
        if(!clickHandlerActive) {
            $("[data-item-type='tweet']").addClass("trash-open-cursor").on("click", tweetClickHandler);
            $("body").addClass("trash-closed-cursor");
            clickHandlerActive = true;
        } else {
            $("[data-item-type='tweet']").removeClass("trash-open-cursor").off("click", tweetClickHandler);
            $("body").removeClass("trash-closed-cursor");
            clickHandlerActive = false;
        }
    });
}); 
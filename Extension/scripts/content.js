$(function() {
    
    var appendImage = function(image, css, node) {
        var imageNode = document.createElement("img");
        imageNode.src = chrome.extension.getURL(image);
        imageNode.classList.add(css);
        document.getElementById(node).appendChild(imageNode);
    }

    $("body").append('<div class="hatebin-logo"></div>');
         
    $.get(chrome.extension.getURL('html/modal.html'), function(data) {
        $($.parseHTML(data)).appendTo('body');
        
       appendImage("images/hatebin_logo_nobg.png", "hatebin-modal-logo", "logoImageHolder1")
       appendImage("images/hatebin_logo_nobg.png", "hatebin-modal-logo", "logoImageHolder2")  
        
        $(".close-reveal-modal").on("click", function() {
            $('#hatebinModalFirst').foundation('reveal', 'close');
        });       
    });
        
    var clickHandlerActive = false;
    
    var tweetClickHandler = function(tweetClickEvent) {
        
        var tweetData = {
            id: $(tweetClickEvent.currentTarget).data("item-id"),
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
        $('#input-other-hate').val('');
        $('#input-love-e-mail').val('');
        $('#input-love-reason').val('');
                 
        $(".hatebin-tweet-content").text(tweetData.text);
                
        $("#hatebinThrowButton").off();
        $("#hatebinThrowButton").on("click", function() {
            $(tweetClickEvent.currentTarget).remove();
            $('#hatebinModalSecond').foundation('reveal', 'open');  
            
            var data = {
                network: "Twitter",
                networkId: tweetData.id,
                text: tweetData.text,
                categories: []
            }
            
            if($("#checkbox-ad-hominem").is(':checked')) {
                data.categories.push("Ad Hominem")
            }            
            if($("#checkbox-sexist").is(':checked')) {
                data.categories.push("Sexist")
            }
            if($("#checkbox-racist").is(':checked')) {
                data.categories.push("Racist")
            }
            var otherReason = $('#input-other-hate').val();
            if($("#checkbox-other-hate").is(':checked') && otherReason.length > 0) {
                data.categories.push(otherReason);
            }

            chrome.runtime.sendMessage({ 
                type: "hate",
                data: JSON.stringify(data)
            });
        });
        
        $("#hatebinSaveButton").off();
        $("#hatebinSaveButton").on("click", function() {
             $('#hatebinModalSecond').foundation('reveal', 'close');
                 
            var loveEmail =  $('#input-love-e-mail').val();
            var loveReason = $('#input-love-reason').val();
                    
            if(loveEmail.length > 0 && loveReason.length > 0) {  
                var data = {
                    email: loveEmail,
                    reason: loveReason
                }

                chrome.runtime.sendMessage({ 
                    type: "love",
                    data: JSON.stringify(data)
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
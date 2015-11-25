$(function(){
	$.ajax({
  		dataType: "json",
  		url: "Account/Token",
  		success: function(response){
			chrome.runtime.sendMessage({
				type: "store", 
				key: "userdata", 
				data: JSON.stringify(response)
			});			
		}
	});
});
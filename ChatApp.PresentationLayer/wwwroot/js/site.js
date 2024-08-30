


window.onload = function () {

    var notSeenMessages = document.querySelectorAll('[data-status = "not-seen"]');   
    var topDiv = null;
    var topPosition = Infinity;
    if (notSeenMessages.length > 0) {

        

        notSeenMessages.forEach(notSeenMessage => {

            const position = notSeenMessage.getBoundingClientRect();
            
            if (position.top < topPosition) {

                topDiv = notSeenMessage;

                topPosition = position.top;
                

            }

            
        });
       


        

        
        topDiv.scrollIntoView({ behavior: "smooth", block: "start", inline: "nearest" });
           

    }
    
    
    
    
    
};




var observer = new IntersectionObserver(entries => {

    entries.forEach(entry => {
        console.log("Observing element:", entry.target.textContent);

        if (entry.isIntersecting) {
            console.log("Element intersecting:", entry.target.textContent);
            entry.target.setAttribute("data-status", "seen");
            var receiverGuid = document.getElementById("hiddenReceiverGuid").value;

            var notificationBox = document.getElementById("notifications-box-" + receiverGuid);

            var currentValue = parseInt(notificationBox.textContent);

            

            currentValue -= 1;

            if (currentValue => 0) {


                

                var messageId = entry.target.getAttribute("data-id");

                $.ajax({

                    type: "POST",
                    url: "/Chat/MessageNotification",
                    dataType: "json",
                    data: { messageId: messageId },

                    success: function (result) {

                        console.log(result);
                    },

                    error: function (req, status, error) {

                        console.log(status);

                    }



                });
                if (currentValue == 0) {

                    notificationBox.remove();
                    observer.disconnect();
                    return;

                }
                notificationBox.textContent = currentValue;
                console.log("Unobserving element:", entry.target.textContent);
                observer.unobserve(entry.target);
            }
         



           
            



        }

    });



}, {
    
    threshold : 1

});

function startObserving() {
    var notSeenMessages = document.querySelectorAll('[data-status="not-seen"]');
    notSeenMessages.forEach(notSeenMsg => {
        console.log("Starting observation for element:", notSeenMsg.textContent); 
        observer.observe(notSeenMsg);
    });
}


document.addEventListener("DOMContentLoaded", startObserving);
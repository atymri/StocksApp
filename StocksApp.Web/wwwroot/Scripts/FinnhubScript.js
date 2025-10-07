//Create a WebSocket to perform duplex (back-and-forth) communication with server
const token = document.querySelector("#FinnhubToken").value;
const socket = new WebSocket(`wss://ws.finnhub.io?token=${token}`);
var stockSymbol = document.getElementById("StockSymbol").value; //get symbol from input hidden
var counter = 0;
var updatedPrice = 0; // global to track latest price

// Connection opened. Subscribe to a symbol
socket.addEventListener('open', function (event) {
    socket.send(JSON.stringify({ 'type': 'subscribe', 'symbol': stockSymbol }));
});

// Listen (ready to receive) for messages
socket.addEventListener('message', function (event) {
    var eventData = JSON.parse(event.data);
    if (eventData && eventData.data) {
        updatedPrice = eventData.data[0].p;
        var timeStamp = eventData.data[0].t;

        //update the chart for every 6 server events
        if (counter == 0 || counter % 6 == 0) {
            prices.push(updatedPrice);
            labels.push(new Date(timeStamp).toLocaleTimeString());
            chart.update();
        }
        counter++;

        //update the UI
        $(".price").text(updatedPrice.toFixed(2)); //price - big display
        $("#price").val($(".price").text());

    }
});

// Unsubscribe
var unsubscribe = function (symbol) {
    socket.send(JSON.stringify({ 'type': 'unsubscribe', 'symbol': symbol }));
}

//when the page is being closed, unsubscribe from the WebSocket
window.onunload = function () {
    unsubscribe(stockSymbol); // fixed typo
};

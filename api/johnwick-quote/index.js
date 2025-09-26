module.exports = async function (context, req) {
    context.log('Processing John Wick quote request.');

    const quotes = [
        { quote: "Yeah, I'm thinking I'm back.", movie: "John Wick (2014)" },
        { quote: "Guns. Lots of guns.", movie: "John Wick: Chapter 3 (2019)" },
        { quote: "Be seeing you.", movie: "John Wick (2014)" },
        { quote: "Consider this a professional courtesy.", movie: "John Wick: Chapter 2 (2017)" },
        { quote: "Everything's got a price.", movie: "John Wick (2014)" },
        { quote: "I'd like to make a reservation for 12.", movie: "John Wick: Chapter 3 (2019)" },
        { quote: "You want a war? Or do you want to just give me a gun?", movie: "John Wick: Chapter 2 (2017)" },
        { quote: "I'm not that guy anymore.", movie: "John Wick (2014)" },
        { quote: "Consequences.", movie: "John Wick: Chapter 3 (2019)" },
        { quote: "Si vis pacem, para bellum.", movie: "John Wick: Chapter 3 (2019)" },
        { quote: "Fortunes favor the bold.", movie: "John Wick: Chapter 4 (2023)" },
        { quote: "I have served. I will be of service.", movie: "John Wick: Chapter 3 (2019)" },
        { quote: "Rules. Without them we'd live with the animals.", movie: "John Wick: Chapter 2 (2017)" },
        { quote: "The marker is sacred.", movie: "John Wick: Chapter 2 (2017)" },
        { quote: "I need something robust. Precise.", movie: "John Wick (2014)" },
        { quote: "No business on Continental grounds.", movie: "John Wick (2014)" },
        { quote: "Tick tock, Mr. Wick.", movie: "John Wick: Chapter 3 (2019)" },
        { quote: "Welcome to the Continental.", movie: "John Wick (2014)" },
        { quote: "A man has to look his best when it's time to get married... or buried.", movie: "John Wick: Chapter 2 (2017)" },
        { quote: "Baba Yaga.", movie: "John Wick (2014)" }
    ];

    // Get a random quote
    const randomQuote = quotes[Math.floor(Math.random() * quotes.length)];

    context.res = {
        status: 200,
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: randomQuote
    };
};

$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: '/Analysis/GetGamesAnalysis',
        success: function (data) {
            const parseTime = d3.timeParse("%Y-%m-%d");
            let times = data.map((d) => (parseTime(d.date.split("T")[0])))
            drawTimeSeariesGraph("games-added-graph", times, "Release date", "Games added")
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.status);
        },
    });
})


$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: '/Analysis/GetOrdersAnalysis',
        success: function (data) {
            const parseTime = d3.timeParse("%Y-%m-%d");
            let times = data.map((d) => (parseTime(d.date.split("T")[0])))
            drawTimeSeariesGraph("orders-graph", times, "Order timeline", "Games Ordered")
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.status);
        },
    });
})

$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: '/Analysis/GetGameReviewAnalysis',
        success: function (data) {
           
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.status);
        },
    });

})
/*
 * Creates a time searies graph
 * Graph that maps data that happend x times to a timeline
 * 
 * Input should be a list of time data objects of d3
 */
function drawTimeSeariesGraph(graphID, times, xAsixName, yAsixName) {
    // Date format in graph disaplay
    var formatDate = d3.timeFormat("%d/%m/%Y");

    times.sort((d1, d2) => (d3.ascending(d1, d2)))

    var xAsixDomain = d3.extent(times, date => (date));

    let GraphWidth = 550
    let GraphHeight = 460
    var margin = { top: 10, right: 30, bottom: 50, left: 60 }
    var width = GraphWidth - margin.left - margin.right
    var height = GraphHeight - margin.top - margin.bottom;

    // append the svg object to the body of the page
    var svg = d3.select(`#${graphID}`)
        .append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");


    // X axis: scale and draw and label
    var xAxis = d3.scaleTime()
        .domain(xAsixDomain)
        .range([0, width])

    svg.append("g")
        .attr("transform", "translate(0," + height + ")")
        .call(d3.axisBottom(xAxis));

    // text label for the x axis
    svg.append("text")
        .attr("style", "color: inhirit;")
        .attr("transform",
            "translate(" + (width / 2) + " ," +
            (height + margin.top + 30) + ")")
        .style("text-anchor", "middle")
        .text(xAsixName);


    // set the parameters for the histogram
    var histogram = d3.histogram()
        .value(date => (date))
        .domain(xAxis.domain())
        .thresholds(xAxis.ticks(70));

    // And apply this function to data to get the bins
    var bins = histogram(times);

    // Y axis: scale and draw and label
    var yAxis = d3.scaleLinear()
        .domain([0, d3.max(bins, function (d) { return d.length; })])
        .range([height, 0]);

    svg.append("g")
        .call(d3.axisLeft(yAxis));

    // text label for the y axis
    svg.append("text")
        .attr("transform", "rotate(-90)")
        .attr("y", 0 - margin.left)
        .attr("x", 0 - (height / 2))
        .attr("dy", "1em")
        .style("text-anchor", "middle")
        .text(yAsixName);

    // Define the div for the tooltip
    var div = d3.select("body").append("div")
        .attr("class", "tooltip")
        .style("opacity", 0);

    // append the bar rectangles to the svg element
    svg.selectAll("rect")
        .data(bins)
        .enter()
        .append("rect")
        .attr("x", 1)
        .attr("transform", function (d) { return "translate(" + xAxis(d.x0) + "," + yAxis(d.length) + ")"; })
        .attr("width", function (d) { return xAxis(d.x1) - xAxis(d.x0) + 1; })
        .attr("height", function (d) { return height - yAxis(d.length); })
        .attr("data-time-start", function (d) { return d.length ? formatDate(d[0]) : ""; })            // Data for toolkip
        .attr("data-time-end", function (d) { return d.length ? formatDate(d[d.length - 1]) : ""; })   // Data for toolkip
        .attr("data-count", function (d) { return d.length })                                              // Data for toolkip
        .on("mouseenter", function (event) {
            let timeStart = event.target.getAttribute("data-time-start")
            let timeEnd = event.target.getAttribute("data-time-end")
            let count = event.target.getAttribute("data-count")

            let tooltipContent = timeStart != timeEnd ? timeStart + "-" + timeEnd : timeStart
            tooltipContent += "<br /> Valus: " + count


            div.html(tooltipContent)
                .style("left", event.pageX + "px")
                .style("top", event.pageY + "px");
            div.transition()
                .duration(20)
                .style("opacity", .9);
        })
        .on("mouseleave", function (d) {
            div.transition()
                .duration(100)
                .style("opacity", 0);
        })
        .style("fill", "#69b3a2")

}




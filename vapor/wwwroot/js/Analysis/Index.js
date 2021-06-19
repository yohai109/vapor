
$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: '/Analysis/GetGamesAnalysis',
        success: function (data) {
            const parseTime = d3.timeParse("%Y-%m-%d");
            let graphData = data.map((d) => ({
                time: parseTime(d.month.split("T")[0]),
                value: d.count
            }))
            drawTimeSeariesGraph("games-added-graph", graphData)
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.status);
        },
    });
})

/*
 * Creates a time searies graph
 * Graph that maps game time to a value
 * 
 * Input should be a list of objects containing time, value fields
 */
function drawTimeSeariesGraph(graphID, graphData) {
    // Date format in graph disaplay
    var formatDate = d3.timeFormat("%d/%m/%Y");

    graphData.sort((d1, d2) => (d3.ascending(d1.time, d2.time)))

    var xAsixDomain = d3.extent(graphData, function (d) { return d.time; });
    var dataYrange = [0, d3.max(graphData, function (d) { return d.value; })];

    var margin = { top: 10, right: 30, bottom: 50, left: 60 }
    var width = 700 - margin.left - margin.right
    var height = 460 - margin.top - margin.bottom;

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
        .text("Month");


    // set the parameters for the histogram
    var histogram = d3.histogram()
        .value(function (d) { return d.time; })
        .domain(xAxis.domain())
        .thresholds(xAxis.ticks(70));

    // And apply this function to data to get the bins
    var bins = histogram(graphData);
    console.log(bins)

    // Y axis: scale and draw and label
    var yAxis = d3.scaleLinear()
        .domain(dataYrange)
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
        .text("Games added");

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
        .attr("transform", function (d) {
            let values = 0
            d.forEach(d => { values += d.value })
            return "translate(" + xAxis(d.x0) + "," + yAxis(values) + ")";
        })
        .attr("width", function (d) { return xAxis(d.x1) - xAxis(d.x0) + 1; })
        .attr("height", function (d) {
            let values = 0
            d.forEach(d => { values += d.value })
            return height - yAxis(values);
        })
        .attr("data-time-start", function (d) {     // Data for tooltip
            console.log(d.length)
            console.log(formatDate(d[0]))
            return d.length ? formatDate(d[0].time) : "";
        })
        .attr("data-time-end", function (d) {       // Data for tooltip
            return d.length ? formatDate(d[d.length - 1].time) : "";
        })
        .attr("data-values", function (d) {         // Data for tooltip                        
            let values = 0
            d.forEach(d => { values += d.value })
            return values
        })
        .on("mouseenter", function (event) {
            let timeStart = event.target.getAttribute("data-time-start")
            let timeEnd = event.target.getAttribute("data-time-end")
            let values = event.target.getAttribute("data-values")

            let tooltipContent = timeStart != timeEnd ? timeStart + "-" + timeEnd : timeStart
            tooltipContent += "<br /> Valus: " + values

            
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




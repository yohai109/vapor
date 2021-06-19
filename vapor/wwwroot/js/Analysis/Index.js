
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

    // append the bar rectangles to the svg element
    svg.selectAll("rect")
        .data(bins)
        .enter()
        .append("rect")
        .attr("x", 1)
        .attr("transform", function (d) {
            let gamesAmount = 0
            d.forEach(d => {
                gamesAmount += d.value
            })
            return "translate(" + xAxis(d.x0) + "," + yAxis(gamesAmount) + ")";
        })
        .attr("width", function (d) { return xAxis(d.x1) - xAxis(d.x0) + 1; })
        .attr("height", function (d) {
            let gamesAmount = 0
            d.forEach(d => {
                gamesAmount += d.value
            })
            return height - yAxis(gamesAmount);
        })
        .style("fill", "#69b3a2")
 
}





$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: '/Analysis/GetGamesAnalysis',
        success: function (data) {
            let graphData = data.map(record => ({
                name: record.month,
                value: record.count
            }))
            drawCategoryGraph("games-added-graph", graphData)
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.status);
        },
    });
})

/*
 * Creates a category graph
 * Graph that maps a name to value (name -> value)
 * 
 * Input: List of dictionaries taht contains name and value fields
 */
function drawCategoryGraph(graphID, graphData) {
    var margin = { top: 10, right: 30, bottom: 50, left: 60 }
    var width = 460 - margin.left - margin.right
    var height = 400 - margin.top - margin.bottom;

    // append the svg object to the body of the page
    var svg = d3.select(`#${graphID}`)
        .append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");


    // X axis scale 
    var xAxis = d3.scaleBand()
        .domain(graphData.map(d => d.name))
        .range([0, width])

    // text label for the x axis
    svg.append("text")
        .attr("style", "color: inhirit;")
        .attr("transform",
            "translate(" + (width / 2) + " ," +
            (height + margin.top + 30) + ")")
        .style("text-anchor", "middle")
        .text("Month");

    svg.append("g")
        .attr("transform", "translate(0," + height + ")")
        .call(d3.axisBottom(xAxis));


    // Y axis: scale and draw:
    var yAxis = d3.scaleLinear()
        .domain([0, d3.max(graphData, function (d) { return d.value; })])
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
        .data(graphData)
        .enter()
        .append("rect")
        .attr("x", 1)
        .attr("transform", function (d) { return "translate(" + xAxis(d.name) + "," + yAxis(d.value) + ")"; })
        .attr("width", function (d) { return width / graphData.length - 10; })
        .attr("height", function (d) { return height - yAxis(d.value); })
        .style("fill", "#69b3a2")


 
}




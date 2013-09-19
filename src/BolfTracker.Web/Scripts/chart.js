/* Wimbledon 2012 - Player Bars */
/* Copyright 2013 Peter Cook (@prcweb); Licensed MIT */

var barHeight = 20, width = 500, numPlayers = 3, parameter = '';
var data = [], yScale = null;
var menu = [
  { id: 'ShotsMade', name: 'Shots Made' },
  { id: 'ShotsMissed', name: 'Shots Missed' },
  { id: 'ShootingPercentage', name: 'Shooting %' },
  { id: 'Points', name: 'Points' },
    { id: 'PointsPerGame', name: 'Points per Game' },
    { id: 'Wins', name: 'Wins' },
    { id: 'Pushes', name: 'Pushes' },
    { id: 'Steals', name: 'Steals' },
    { id: 'SugarFreeSteals', name: 'Sugar Free' }
];

//// HELPER FUNCTIONS
function px(s) {
    return s + 'px';
}


//// UI
function getChart(d, generate) {
    if (parameter === d)
        return;

    parameter = d;
    GetData(generate);
}


//// D3
function updateScale() {
    yScale = d3.scale.linear()
      .domain([0, d3.max(data, function (d) {
           return d[parameter];
      })])
      .range([0, width]);
}

function updateChart(generate) {
    //updateScale();
    
    if (generate)
        generateChart();
    
    d3.select('#chart')
      .selectAll('div.bar')
      .transition()
      .duration(1000)
      .style('width', function (d) {
          return px(yScale(d[parameter]));
      });

    d3.select('#chart')
      .selectAll('div.value')
      .transition()
      .duration(1000)
      .tween("text", function (d) {
          // Thanks to http://stackoverflow.com/questions/13454993/increment-svg-text-with-transistion
          var i = d3.interpolate(this.textContent, d[parameter]);
          return function (t) {
              this.textContent = Math.round(i(t));
          };
      });
}

function generateChart() {
    //data = filterPlayers(csv);

    //$("#chart").empty();
    updateScale();

    var players = d3.select('#chart')
      .selectAll('div')
      .data(data)
      .enter()
      .append('div')
      .sort(function (a, b) {
           return d3.descending(a[parameter], b[parameter]);
      })
      .classed('player', true)
      .style('top', function (d, i) {
          return px(i * barHeight);
      });

    players.append('div')
      .classed('label', true)
      .text(function (d) {
           return d.Name;
      });

    players.append('div')
      .classed('bar', true)
      .style('height', px(barHeight * 0.95))
      .style('width', function (d) {
          return px(yScale(d[parameter]));
      });

    players.append('div')
      .classed('value', true)
      .text(function (d) { return (d[parameter]); });

    //// Menu
    //d3.select('#menu')
    //  .selectAll('div')
    //  .data(menu)
    //  .enter()
    //  .append('div')
    //  .text(function (d) { return d.name; })
    //  .classed('selected', function (d, i) { return i == 0; })
    //  .on('click', menuClick);
}

 function GetData(generate) {
    $.ajax({
        url: "/Player/GetPlayerStatisticsJson",
        type: "GET",
        success: function (result) {
            data=result;
            updateChart(generate);
        },
        error: function (err) {
            self.Result("An error occured while processing your request. Please try again later.");
        }
    });
};
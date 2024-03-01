let activeChart;

$('#mostSoldMenus').click(function () {
    if (activeChart) {
        activeChart.destroy();
    }
    $.ajax({
        url: '@Url.Action("getMostSoldMenus", "Statistic")',
        dataType: 'json',
        type: 'GET',
        success: function (soldMenus) {
            activeChart = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: ['Menü 1', 'Menü 2', 'Menü 3'],
                    datasets: [{
                        data: soldMenus.value
                    }]
                },
                options: {
                    aspectRatio: 3
                }
            });
        },
        error: function () {
            alert('Error');
        }
    });
});

$('#soldMenus').click(function () {
    if (activeChart) {
        activeChart.destroy();
    }
    $.ajax({
        url: '@Url.Action("getIrgendwas", "Statistic")',
        dataType: 'json',
        type: 'GET',
        success: function (test) {
            let x = [];
            let Menu1_y = [];
            let Menu2_y = [];
            let Menu3_y = [];
            for (let data of test.value[0]) {
                x.push(data.x);
                Menu1_y.push(data.y);
            }
            for (let data of test.value[1]) {
                Menu2_y.push(data.y);
            }
            for (let data of test.value[2]) {
                Menu3_y.push(data.y);
            }
            activeChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: x,
                    datasets: [{
                        label: 'Menü 1',
                        data: Menu1_y
                    },
                    {
                        label: 'Menü 2',
                        data: Menu2_y
                    },
                    {
                        label: 'Menü 3',
                        data: Menu3_y
                    }]
                },
                options: {
                    aspectRatio: 4,
                    scales: {
                        y: {
                            ticks: {
                                stepSize: 1,
                            }
                        }
                    }
                }
            });
        },
        error: function () {
            alert('Error');
        }
    });
});

const ctx = document.getElementById('chart');
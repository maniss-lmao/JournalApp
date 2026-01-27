window.journalCharts = {
    moodPie: null,
    tagsBar: null,
    wordLine: null,

    renderMoodPie: function (canvasId, labels, values) {
        const ctx = document.getElementById(canvasId);
        if (!ctx) return;

        if (this.moodPie) this.moodPie.destroy();

        this.moodPie = new Chart(ctx, {
            type: "pie",
            data: {
                labels: labels,
                datasets: [{
                    data: values
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: { position: "bottom" }
                }
            }
        });
    },

    renderTagsBar: function (canvasId, labels, values) {
        const ctx = document.getElementById(canvasId);
        if (!ctx) return;

        if (this.tagsBar) this.tagsBar.destroy();

        this.tagsBar = new Chart(ctx, {
            type: "bar",
            data: {
                labels: labels,
                datasets: [{
                    label: "Tag usage",
                    data: values
                }]
            },
            options: {
                responsive: true,
                plugins: { legend: { display: false } },
                scales: {
                    y: { beginAtZero: true }
                }
            }
        });
    },

    renderWordTrend: function (canvasId, labels, values) {
        const ctx = document.getElementById(canvasId);
        if (!ctx) return;

        if (this.wordLine) this.wordLine.destroy();

        this.wordLine = new Chart(ctx, {
            type: "line",
            data: {
                labels: labels,
                datasets: [{
                    label: "Words per day",
                    data: values,
                    tension: 0.25
                }]
            },
            options: {
                responsive: true,
                plugins: { legend: { position: "bottom" } },
                scales: {
                    y: { beginAtZero: true }
                }
            }
        });
    }
};

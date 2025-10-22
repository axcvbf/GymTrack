const exerciseGroups = {
    push: [1, 4, 3],
    pull: [8, 7, 6],
    legs: [5, 2, 9]
};

const exerciseLabels = {
    1: "Bench Press",
    2: "Quads",
    3: "Shoulder Press",
    4: "Incline Bench Press",
    5: "Legpress",
    6: "Pull Up",
    7: "Seated Row",
    8: "Bicep Curl",
    9: "Calves"
};

let charts = [];
async function loadCharts(group) {
    charts.forEach(c => c.destroy());
    charts = [];

    const exerciseIds = exerciseGroups[group];
    const container = document.getElementById("graphs");
    container.innerHTML = "";

    const canvases = exerciseIds.map(id => {
        const canvas = document.createElement("canvas");
        canvas.id = `chart-${id}`;
        container.appendChild(canvas);
        return { id, canvas };
    });

    const fetchPromises = canvases.map(({ id }) =>
        fetch(`api/ApiStats/${id}`)
            .then(res => res.json())
            .then(data => ({ id, data }))
            .catch(error => ({ id, error }))
    );

    const results = await Promise.all(fetchPromises);

    results.forEach(({ id, data, error }) => {
        if (error) {
            console.error(`Error chart for ID ${id}:`, error);
            return;
        }

        const labels = data.map(d => d.date);
        const values = data.map(d => d.weight);

        const canvas = document.getElementById(`chart-${id}`);
        const ctx = canvas.getContext("2d");

        const chart = new Chart(ctx, {
            type: "line",
            data: {
                labels: labels,
                datasets: [{
                    label: exerciseLabels[id],
                    data: values,
                    borderWidth: 2,
                    borderColor: "black",
                    backgroundColor: "rgba(0,0,0,0.1)",
                    tension: 0.3,
                    pointRadius: 2,
                    pointBackgroundColor: "black"
                }]
            },
            options: {
                responsive: false,
                maintainAspectRatio: false,
                aspectRatio: 1.6,
                plugins: {
                    legend: {
                        display: true,
                        labels: {
                            boxWidth: 0,
                            color: 'black'
                        }
                    }
                },
                scales: {
                    x: { title: { display: true, text: "Date" } },
                    y: { title: { display: true, text: "Weight (kg)" }, beginAtZero: false }
                }
            }
        });
        charts.push(chart);
    });
}
document.getElementById("exerciseGroup").addEventListener("change", (e) => { 
    loadCharts(e.target.value);
})

loadCharts("push");
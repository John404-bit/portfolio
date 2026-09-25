const list = document.getElementById("projects");

fetch("/api/project")
  .then(r => r.json())
  .then(projects => {
    for (const p of projects) {
      const li = document.createElement("li");
      li.textContent = `${p.title} – ${p.description}`;
      list.append(li);
    }
  });

// ---------- Projekter fra API'et ----------
const list = document.getElementById("projects");
const message = document.getElementById("status");

fetch("/api/project")
  .then(r => {
    if (!r.ok) throw new Error(r.status); // fetch fejler ikke selv ved 404/500
    return r.json();
  })
  .then(projects => {
    if (projects.length === 0) {
      message.textContent = "No projects yet.";
      return;
    }
    message.remove();

    for (const p of projects) {
      const li = document.createElement("li");
      li.className = "card";

      // textContent i stedet for innerHTML, så tekst fra databasen aldrig køres som HTML
      const title = document.createElement("h3");
      title.textContent = p.title;

      const description = document.createElement("p");
      description.textContent = p.description;

      const date = document.createElement("time");
      date.dateTime = p.createdUtc;
      date.textContent = new Date(p.createdUtc).toLocaleDateString("en-GB", {
        day: "numeric", month: "long", year: "numeric"
      });

      li.append(title, description, date);
      list.append(li);
    }
  })
  .catch(() => {
    message.textContent = "Could not load projects. Is the API running?";
  });

// ---------- Øjne der følger musen ----------
const eyes = document.querySelector(".eyes");

document.addEventListener("mousemove", e => {
  const box = eyes.getBoundingClientRect();
  const dx = e.clientX - (box.left + box.width / 2);  // afstand fra øjnenes midte til musen
  const dy = e.clientY - (box.top + box.height / 2);
  const angle = Math.atan2(dy, dx);                    // retningen mod musen

  eyes.style.setProperty("--x", Math.cos(angle) * 3 + "px");
  eyes.style.setProperty("--y", Math.sin(angle) * 5 + "px");
});

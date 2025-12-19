// Using port 7149 based on your previous screenshot
const API_URL = "https://localhost:7149/api/auth";

// --- REGISTER FUNCTION ---
async function registerUser() {
    const firstName = document.getElementById("regFirstName").value;
    const lastName = document.getElementById("regLastName").value;
    const email = document.getElementById("regEmail").value;
    const password = document.getElementById("regPassword").value;

    const data = { firstName, lastName, email, password };

    try {
        const response = await fetch(`${API_URL}/register`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        });

        if (response.ok) {
            alert("Registration Successful! Now please Log In.");
        } else {
            const errorText = await response.text();
            alert("Registration Failed: " + errorText);
        }
    } catch (error) {
        console.error("Error:", error);
        alert("Could not connect to server.");
    }
}

// --- LOGIN FUNCTION ---
async function loginUser() {
    const email = document.getElementById("loginEmail").value;
    const password = document.getElementById("loginPassword").value;

    const data = { email, password };

    try {
        const response = await fetch(`${API_URL}/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        });

        const result = await response.json();

        if (response.ok) {
            alert("LOGIN SUCCESS! Welcome " + result.user);

            // 1. SAVE THE USER ID (Important for the dashboard)
            localStorage.setItem("userId", result.userId);

            // 2. REDIRECT TO DASHBOARD
            window.location.href = "dashboard.html";
        } else {
            alert("Login Failed: " + JSON.stringify(result));
        }
    } catch (error) {
        console.error("Error:", error);
        alert("Could not connect to server.");
    }
}
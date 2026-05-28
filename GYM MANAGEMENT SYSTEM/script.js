// Toast Notification
function showToast(message) {
    const toast = document.getElementById("toast");
    toast.innerText = message;
    toast.classList.add("show");

    setTimeout(() => {
        toast.classList.remove("show");
    }, 3000);
}

// Registration Form
document.getElementById("registerForm").addEventListener("submit", function (e) {
    e.preventDefault();

    const name = document.getElementById("name").value.trim();
    const email = document.getElementById("email").value.trim();
    const phone = document.getElementById("phone").value.trim();
    const membership = document.getElementById("membership").value;
    const successMsg = document.getElementById("successMsg");

    if (!name || !email || !phone) {
        showToast("Please fill all fields");
        return;
    }

    const emailPattern = /^[^ ]+@[^ ]+\.[a-z]{2,3}$/;
    const phonePattern = /^[0-9]{10}$/;

    if (!emailPattern.test(email)) {
        showToast("Enter a valid email");
        return;
    }

    if (!phonePattern.test(phone)) {
        showToast("Enter a valid 10-digit phone number");
        return;
    }

    successMsg.innerText =
        `Welcome ${name}! Registration successful for ${membership} plan.`;

    showToast("Registration Successful 🎉");

    this.reset();
});

// BMI Calculator
function calculateBMI() {
    const weight = parseFloat(document.getElementById("weight").value);
    const height = parseFloat(document.getElementById("height").value);
    const bmiResult = document.getElementById("bmiResult");

    if (!weight || !height || height <= 0) {
        showToast("Enter valid weight and height");
        return;
    }

    const bmi = (weight / (height * height)).toFixed(2);

    let category = "";

    if (bmi < 18.5) {
        category = "Underweight";
    } else if (bmi < 25) {
        category = "Normal Weight";
    } else if (bmi < 30) {
        category = "Overweight";
    } else {
        category = "Obese";
    }

    bmiResult.innerText = `Your BMI is ${bmi} (${category})`;

    showToast("BMI Calculated Successfully");
}

// Counter Animation
const counters = document.querySelectorAll(".counter");

const startCounter = () => {
    counters.forEach(counter => {
        const target = +counter.getAttribute("data-target");
        let count = 0;
        const increment = target / 100;

        const updateCounter = () => {
            if (count < target) {
                count += increment;
                counter.innerText = Math.ceil(count);
                setTimeout(updateCounter, 20);
            } else {
                counter.innerText = target + "+";
            }
        };

        updateCounter();
    });
};

// Trigger Counter on Scroll
let counterStarted = false;

window.addEventListener("scroll", () => {
    const statsSection = document.getElementById("stats");
    const sectionTop = statsSection.offsetTop - 300;

    if (window.scrollY > sectionTop && !counterStarted) {
        startCounter();
        counterStarted = true;
    }
});

// Mobile Menu Toggle
const menuBtn = document.querySelector(".menu-btn");
const navLinks = document.querySelector(".nav-links");

menuBtn.addEventListener("click", () => {
    if (navLinks.style.display === "flex") {
        navLinks.style.display = "none";
    } else {
        navLinks.style.display = "flex";
        navLinks.style.flexDirection = "column";
        navLinks.style.position = "absolute";
        navLinks.style.top = "80px";
        navLinks.style.right = "20px";
        navLinks.style.background = "#111";
        navLinks.style.padding = "20px";
        navLinks.style.borderRadius = "12px";
        navLinks.style.gap = "15px";
    }
});

// Smooth Navbar Close After Click
document.querySelectorAll(".nav-links a").forEach(link => {
    link.addEventListener("click", () => {
        if (window.innerWidth <= 768) {
            navLinks.style.display = "none";
        }
    });
});

// Button Plan Selection
document.querySelectorAll(".card button").forEach(button => {
    button.addEventListener("click", () => {
        showToast("Plan selected! Register now.");
        document.getElementById("register").scrollIntoView({
            behavior: "smooth"
        });
    });
});

// Hero Animation
window.addEventListener("load", () => {
    document.querySelector(".hero-content").style.opacity = "1";
    document.querySelector(".hero-content").style.transform = "translateY(0)";
});
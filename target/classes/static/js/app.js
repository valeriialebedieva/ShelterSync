const API_URL = '/api/animals';
let allPets = [];

document.addEventListener('DOMContentLoaded', () => {
    if (document.getElementById('gallery-grid')) {
        loadGallery();
        document.getElementById('search-input').addEventListener('input', filterGallery);
        document.getElementById('filter-status').addEventListener('change', filterGallery);
        document.getElementById('filter-breed').addEventListener('change', filterGallery);
    }
    if (document.getElementById('admin-table-body')) {
        loadAdminTable();
        document.getElementById('add-pet-form').addEventListener('submit', handleAddPet);
    }
});

async function loadGallery() {
    try {
        const response = await fetch(API_URL);
        allPets = await response.json();
        renderGallery(allPets);
    } catch (error) {
        console.error('Error loading gallery:', error);
    }
}

function renderGallery(pets) {
    const grid = document.getElementById('gallery-grid');
    grid.innerHTML = '';

    pets.forEach(pet => {
        const card = document.createElement('div');
        card.className = 'pet-card';
        card.innerHTML = `
            <div class="pet-image">
                ${pet.imageUrl ? `<img src="${pet.imageUrl}" alt="${pet.name}" style="width:100%;height:100%;object-fit:cover;">` : 'No Image'}
            </div>
            <div class="pet-info">
                <div style="display:flex;justify-content:space-between;align-items:start;">
                    <h3>${pet.name}</h3>
                    <span class="status-badge">${pet.status}</span>
                </div>
                <p><strong>Breed:</strong> ${pet.breed}</p>
                <p><strong>Age:</strong> ${pet.age} years</p>
                <p><strong>Arrived:</strong> ${pet.arrivalDate}</p>
            </div>
        `;
        grid.appendChild(card);
    });
}

function filterGallery() {
    const searchTerm = document.getElementById('search-input').value.toLowerCase();
    const statusFilter = document.getElementById('filter-status').value;
    const breedFilter = document.getElementById('filter-breed').value;

    const filtered = allPets.filter(pet => {
        const matchesName = pet.name.toLowerCase().includes(searchTerm);
        const matchesStatus = statusFilter === 'All' || pet.status === statusFilter;
        
        let matchesBreed = true;
        if (breedFilter === 'Cat') {
            matchesBreed = pet.breed.toLowerCase().includes('cat') || pet.breed.toLowerCase().includes('tabby');
        } else if (breedFilter === 'Dog') {
            matchesBreed = !pet.breed.toLowerCase().includes('cat') && !pet.breed.toLowerCase().includes('tabby'); 
        }

        return matchesName && matchesStatus && matchesBreed;
    });

    renderGallery(filtered);
}

async function loadAdminTable() {
    try {
        const response = await fetch(API_URL);
        const pets = await response.json();
        const tbody = document.getElementById('admin-table-body');
        tbody.innerHTML = '';
        
        if(document.getElementById('total-count')) {
            document.getElementById('total-count').innerText = pets.length;
        }

        pets.forEach(pet => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${pet.id}</td>
                <td>${pet.name}</td>
                <td>${pet.breed}</td>
                <td>${pet.status}</td>
                <td>
                    <button class="btn btn-delete" onclick="deletePet(${pet.id})">Delete</button>
                </td>
            `;
            tbody.appendChild(row);
        });
    } catch (error) {
        console.error('Error loading admin table:', error);
    }
}

async function handleAddPet(event) {
    event.preventDefault();
    
    const petData = {
        name: document.getElementById('name').value,
        breed: document.getElementById('breed').value,
        age: parseInt(document.getElementById('age').value),
        status: document.getElementById('status').value,
        arrivalDate: document.getElementById('arrivalDate').value,
        imageUrl: document.getElementById('imageUrl').value
    };

    try {
        const response = await fetch(API_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(petData)
        });

        if (response.ok) {
            alert('Pet added successfully!');
            document.getElementById('add-pet-form').reset();
            loadAdminTable();
        } else {
            alert('Error adding pet');
        }
    } catch (error) {
        console.error('Error:', error);
    }
}

async function deletePet(id) {
    if (confirm('Are you sure you want to delete this record?')) {
        try {
            const response = await fetch(`${API_URL}/${id}`, {
                method: 'DELETE'
            });
            if (response.ok) {
                loadAdminTable();
            }
        } catch (error) {
            console.error('Error deleting pet:', error);
        }
    }
}

function calculateBudget() {
    const count = parseInt(document.getElementById('total-count').innerText);
    const cost = parseInt(document.getElementById('cost-per-pet').value);
    const total = count * cost;
    document.getElementById('budget-result').innerText = `Total Estimated Weekly Budget: $${total}`;
}
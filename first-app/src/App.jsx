import { useState, useEffect } from 'react'
import './App.css'

const API_BASE = 'http://localhost:5132/api'

function App() {
  const [currentTab, setCurrentTab] = useState('parts')
  const [parts, setParts] = useState([])
  const [cars, setCars] = useState([])
  const [partForm, setPartForm] = useState({ name: '', category: '', price: '' })
  const [carForm, setCarForm] = useState({ make: '', model: '', year: '' })
  const [editingPart, setEditingPart] = useState(null)
  const [editingCar, setEditingCar] = useState(null)

  useEffect(() => {
    if (currentTab === 'parts') {
      fetchParts()
    } else {
      fetchCars()
    }
  }, [currentTab])

  const fetchParts = async () => {
    try {
      const response = await fetch(`${API_BASE}/parts`)
      const data = await response.json()
      setParts(data)
    } catch (error) {
      console.error('Error fetching parts:', error)
    }
  }

  const fetchCars = async () => {
    try {
      const response = await fetch(`${API_BASE}/cars`)
      const data = await response.json()
      setCars(data)
    } catch (error) {
      console.error('Error fetching cars:', error)
    }
  }

  const handlePartChange = (e) => {
    setPartForm({ ...partForm, [e.target.name]: e.target.value })
  }

  const handleCarChange = (e) => {
    setCarForm({ ...carForm, [e.target.name]: e.target.value })
  }

  const handlePartSubmit = async (e) => {
    e.preventDefault()
    if (partForm.name && partForm.category && partForm.price) {
      try {
        const method = editingPart ? 'PUT' : 'POST'
        const url = editingPart ? `${API_BASE}/parts/${editingPart.id}` : `${API_BASE}/parts`
        const response = await fetch(url, {
          method,
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            name: partForm.name,
            category: partForm.category,
            price: parseFloat(partForm.price)
          })
        })
        if (response.ok) {
          fetchParts()
          setPartForm({ name: '', category: '', price: '' })
          setEditingPart(null)
        }
      } catch (error) {
        console.error('Error saving part:', error)
      }
    }
  }

  const handleCarSubmit = async (e) => {
    e.preventDefault()
    if (carForm.make && carForm.model && carForm.year) {
      try {
        const method = editingCar ? 'PUT' : 'POST'
        const url = editingCar ? `${API_BASE}/cars/${editingCar.id}` : `${API_BASE}/cars`
        const response = await fetch(url, {
          method,
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            make: carForm.make,
            model: carForm.model,
            year: parseInt(carForm.year)
          })
        })
        if (response.ok) {
          fetchCars()
          setCarForm({ make: '', model: '', year: '' })
          setEditingCar(null)
        }
      } catch (error) {
        console.error('Error saving car:', error)
      }
    }
  }

  const handleEditPart = (part) => {
    setEditingPart(part)
    setPartForm({ name: part.name, category: part.category, price: part.price.toString() })
  }

  const handleEditCar = (car) => {
    setEditingCar(car)
    setCarForm({ make: car.make, model: car.model, year: car.year.toString() })
  }

  const handleDeletePart = async (id) => {
    try {
      const response = await fetch(`${API_BASE}/parts/${id}`, { method: 'DELETE' })
      if (response.ok) {
        fetchParts()
      }
    } catch (error) {
      console.error('Error deleting part:', error)
    }
  }

  const handleDeleteCar = async (id) => {
    try {
      const response = await fetch(`${API_BASE}/cars/${id}`, { method: 'DELETE' })
      if (response.ok) {
        fetchCars()
      }
    } catch (error) {
      console.error('Error deleting car:', error)
    }
  }

  return (
    <div className="app">
      <h1>🚗 Менеджер автомобилей и запчастей 🔧</h1>
      
      <div className="tabs">
        <button 
          className={`tab-btn ${currentTab === 'cars' ? 'active' : ''}`} 
          onClick={() => setCurrentTab('cars')}
        >
          🚗 Автомобили
        </button>
        <button 
          className={`tab-btn ${currentTab === 'parts' ? 'active' : ''}`} 
          onClick={() => setCurrentTab('parts')}
        >
          🔧 Запчасти
        </button>
      </div>

      {currentTab === 'parts' && (
        <>
          <form onSubmit={handlePartSubmit} className="part-form">
            <div className="form-group">
              <label>Название:</label>
              <input
                type="text"
                name="name"
                value={partForm.name}
                onChange={handlePartChange}
                required
              />
            </div>
            <div className="form-group">
              <label>Категория:</label>
              <input
                type="text"
                name="category"
                value={partForm.category}
                onChange={handlePartChange}
                required
              />
            </div>
            <div className="form-group">
              <label>Цена:</label>
              <input
                type="number"
                name="price"
                value={partForm.price}
                onChange={handlePartChange}
                step="0.01"
                required
              />
            </div>
            <button type="submit" className="submit-btn">
              {editingPart ? '✏️ Обновить' : '➕ Добавить'} запчасть
            </button>
          </form>

          <table className="parts-table">
            <thead>
              <tr>
                <th>Название</th>
                <th>Категория</th>
                <th>Цена</th>
                <th>Действия</th>
              </tr>
            </thead>
            <tbody>
              {parts.map(part => (
                <tr key={part.id}>
                  <td>{part.name}</td>
                  <td>{part.category}</td>
                  <td>{part.price} руб.</td>
                  <td>
                    <button className="edit-btn" onClick={() => handleEditPart(part)}>✏️</button>
                    <button className="delete-btn" onClick={() => handleDeletePart(part.id)}>🗑️</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </>
      )}

      {currentTab === 'cars' && (
        <>
          <form onSubmit={handleCarSubmit} className="part-form">
            <div className="form-group">
              <label>Марка:</label>
              <input
                type="text"
                name="make"
                value={carForm.make}
                onChange={handleCarChange}
                required
              />
            </div>
            <div className="form-group">
              <label>Модель:</label>
              <input
                type="text"
                name="model"
                value={carForm.model}
                onChange={handleCarChange}
                required
              />
            </div>
            <div className="form-group">
              <label>Год:</label>
              <input
                type="number"
                name="year"
                value={carForm.year}
                onChange={handleCarChange}
                required
              />
            </div>
            <button type="submit" className="submit-btn">
              {editingCar ? '✏️ Обновить' : '➕ Добавить'} автомобиль
            </button>
          </form>

          <table className="parts-table">
            <thead>
              <tr>
                <th>Марка</th>
                <th>Модель</th>
                <th>Год</th>
                <th>Действия</th>
              </tr>
            </thead>
            <tbody>
              {cars.map(car => (
                <tr key={car.id}>
                  <td>{car.make}</td>
                  <td>{car.model}</td>
                  <td>{car.year}</td>
                  <td>
                    <button className="edit-btn" onClick={() => handleEditCar(car)}>✏️</button>
                    <button className="delete-btn" onClick={() => handleDeleteCar(car.id)}>🗑️</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </>
      )}
    </div>
  )
}

export default App

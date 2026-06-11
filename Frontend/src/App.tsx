import { useState } from 'react'

import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './App.css'

//Pages
import Index from "./pages/Index";
import Test from "./pages/Test";
import Results from "./pages/TestResults";



function App() {


  return (
   <BrowserRouter>
   <Routes>
    
    <Route path="/" element={<Index/>}/>
    <Route path="/Test" element={<Test/>}/>
    <Route path="/Results" element={<Results/>}/>
    <Route path="/results/:sessionId" element={<Results />} />
   </Routes>
   </BrowserRouter>
  )
}

export default App

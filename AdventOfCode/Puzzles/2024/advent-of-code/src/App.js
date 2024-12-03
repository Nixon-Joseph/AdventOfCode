import { BrowserRouter, Routes, Route } from "react-router-dom";
import Home from "./Pages/Home";
import Day from "./Pages/Day";
import './App.css';
import Layout from "./Layout";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<Home />} />
          <Route path="day/:day" element={<Day />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;

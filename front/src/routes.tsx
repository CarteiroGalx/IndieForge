import { createBrowserRouter } from "react-router-dom";
import Login from "./pages/Login/Login"
import Register from "./pages/Register/Register";
import Home from "./pages/Home/Home";
import MyProfile from "./pages/MyProfile/MyProfile";

const router = createBrowserRouter([
  {
    path: "/", 
    element: <Login />, 
  },
  {
    path: "/register", 
    element: <Register />,
  },
  {
    path: "/home",
    element: <Home />,
  },
  {
    path: "/profile",
    element: <MyProfile />,
  }
]);

export default router;

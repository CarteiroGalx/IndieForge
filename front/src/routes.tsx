import { createBrowserRouter } from "react-router-dom";
import Login from "./pages/Login"
import Register from "./pages/Register";
import Home from "./pages/Home";
import MyProfile from "./pages/MyProfile";

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

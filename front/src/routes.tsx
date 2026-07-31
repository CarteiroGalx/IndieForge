import { createBrowserRouter } from "react-router-dom";
import Login from "./pages/Login/Login"
import Register from "./pages/Register/Register";
import Home from "./pages/Home/Home";
import MyProfile from "./pages/MyProfile/MyProfile";
import ProjectDetails from "./pages/ProjectDetails/ProjectDetails";

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
  },
  {
    path: "/projects/:projectId",
    element: <ProjectDetails />,
  }
]);

export default router;

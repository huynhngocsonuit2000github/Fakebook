import React from 'react';
import { Login } from './pages/Account/Login';

import './styles/template/css/index.css'
import { Route, BrowserRouter as Router, Routes } from 'react-router-dom';
import { AdminLayout } from './Layout/AdminLayout/AdminLayout';
import { Dashboard } from './pages/Admin/Dashboard';
import { Settings } from './pages/Admin/Settings';
import { MemberLayout } from './Layout/MemberLayout/MemberLayout';
import { Home } from './pages/Member/Home';
import { Friends } from './pages/Member/Friends';
import PrivateRoute from './components/PrivateRoute/PrivateRoute';

function App() {
  return (
    <div className="App">
      <Router>
        <Routes>
          <Route path="/login" element={<Login />}></Route>

          <Route path="/admin" element={<PrivateRoute requiredPermissions={['admin_read']} element={<AdminLayout />} />}>
            <Route index element={<Dashboard />} />
            <Route path='dashboard' element={<Dashboard />}></Route>
            <Route path='settings' element={<PrivateRoute requiredPermissions={['admin_create']} element={<Settings />} />}></Route>
          </Route>


          <Route path="/" element={<PrivateRoute requiredPermissions={['member_read']} element={<MemberLayout />} />}>
            <Route index element={<Home />} />
            <Route path='' element={<Home />}></Route>
            <Route path='friends' element={<PrivateRoute requiredPermissions={['member_create']} element={<Friends />} />}></Route>
          </Route>


        </Routes>
      </Router>
    </div>
  );
}

export default App;

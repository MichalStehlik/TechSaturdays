import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import { FrontLayout } from "./pages/Front"
import Home from "./pages/Front/Home"
import { AccountLayout } from "./pages/Account"
import Profile from "./pages/Account/Profile"
import SignIn from "./pages/Account/SignIn"
import SignUp from "./pages/Account/SignUp"
import NotFound from "./pages/Special/NotFound"
import './custom.css';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
        <Routes>
          <Route path='/' element={<FrontLayout />}>
            <Route index element={<Home />} />
          </Route>
          <Route path='/account' element={<AccountLayout />}>
            <Route index element={<Profile />} />
            <Route path="sign-in" element={<SignIn />} />
            <Route path="sign-up" element={<SignUp />} />
          </Route>
          <Route path="*" element={<NotFound />} />
        </Routes>
    );
  }
}

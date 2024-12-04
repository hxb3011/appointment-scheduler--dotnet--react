import React from "react";
import { Link, BrowserRouter, Route, Routes, useLocation } from "react-router-dom";

import { Icon } from "./assets/icons/mdi";

import { AccountCard, Card, NavCard } from "./components/Card";

import { Home } from "./pages/HomePage";
import { Profile } from "./pages/ProfilePage";
import { Appointment } from "./pages/AppointmentPage";
import { Scheduler } from "./pages/SchedulerPage";

import logo from "./logo.svg";

import { FontFamily } from "./assets/fonts/noto"

import "./App.css";
import { Login } from "./pages/LoginPage";
import { Register } from "./pages/RegisterPage";
import { ProfileEditor } from "./pages/ProfileEditorPage";

function AppContent() {
  const location = useLocation();
  return (
    <div className="App" mdc-theme="system">
      <Card heading="Phòng khám Khoa Niên" attributes={{
        style: { width: "100%" }
      }} />
      {(location.pathname.startsWith("/login")
        || location.pathname.startsWith("/register")
      ) ? '' : (<>
        <AccountCard name="Huynh Xuan Bach" username="hxb3011" imageURL="/favicon.ico" />
        <NavCard navigations={[
          { activeIcon: Icon.calendar_multiselect, icon: Icon.calendar_multiselect_outline, name: "Đặt lịch", url: "/schedule" },
          { activeIcon: Icon.history, icon: Icon.history, name: "Lịch đã đặt", url: "/appointment" },
          { activeIcon: Icon.account_box, icon: Icon.account_box_outline, name: "Hồ sơ", url: "/profile" },
          { activeIcon: Icon.information, icon: Icon.information_outline, name: "Giới thiệu", url: "/" }
        ]} />
      </>)}
      <Routes>
        <Route index path="/" element={<Home />} />
        <Route path="/profile" element={<Profile />} />
        <Route path="/profile/create" element={<ProfileEditor />} />
        <Route path="/appointment" element={<Appointment />} />
        <Route path="/schedule" element={<Scheduler />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
      </Routes>
    </div>
  );
}

export function App() {
  return (
    <BrowserRouter>
      <AppContent />
    </BrowserRouter>
  );
}
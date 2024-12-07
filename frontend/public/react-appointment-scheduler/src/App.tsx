import React, { useEffect, useState } from "react";
import { Link, BrowserRouter, Route, Routes, useLocation, useNavigate } from "react-router-dom";

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
import { currentUser, Patient } from "./services/patient";
import { getAccessToken, setAccessToken } from "./services/auth";
import { apiServer } from "./utils/api";

function AppContent() {
  const location = useLocation();
  const navigate = useNavigate();
  const [user, setUser] = useState<Patient>();
  useEffect(() => {
    currentUser().then(value => {
      if (value.type == "ok") {
        setUser(value);
        return;
      }
      setAccessToken();
      setUser(undefined);
    }, reason => {
      setAccessToken();
      setUser(undefined);
    });
  }, [getAccessToken()])

  return (
    <div className="App" mdc-theme="system">
      <Card heading="Phòng khám Khoa Niên" attributes={{
        style: { width: "100%" }
      }} />
      {(location.pathname.startsWith("/login")
        || location.pathname.startsWith("/register")
      ) ? '' : (<>
        <AccountCard
          user={user}
          name={user && user.full_name}
          username={(user && (user.username || user.phone)) || "Bạn cần đăng nhập để thực hiện chức năng đặt lịch, xem lịch đã đặt, ..."}
          imageURL={user && (user.image || user.id ? apiServer + "user/current/image" : "/favicon.ico")}
          actionAttributes={{
            async onClick() {
              if (!user) {
                await navigate("/login");
              } else {
                setAccessToken();
                setUser(undefined);
                await navigate("/");
              }
            }
          }} />
        <NavCard user={user} key={user && user.id || "_"} navigations={[
          { activeIcon: Icon.calendar_multiselect, icon: Icon.calendar_multiselect_outline, name: "Đặt lịch", url: user ? "/schedule" : "/login?redirect=%2Fschedule" },
          { activeIcon: Icon.history, icon: Icon.history, name: "Lịch đã đặt", url: user ? "/appointment" : "/login?redirect=%2Fappointment" },
          { activeIcon: Icon.account_box, icon: Icon.account_box_outline, name: "Hồ sơ", url: user ? "/profile" : "/login?redirect=%2Fprofile" },
          { activeIcon: Icon.information, icon: Icon.information_outline, name: "Giới thiệu", url: "/" }
        ]} />
      </>)}
      <Routes>
        <Route index={!user} path="/" element={<Home />} />
        {!user ? '' : (<>
          <Route index={!!user} path="/profile" element={<Profile />} />
          <Route path="/profile/create" element={<ProfileEditor />} />
          <Route path="/appointment" element={<Appointment />} />
          <Route path="/schedule" element={<Scheduler />} />
        </>)}
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
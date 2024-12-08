import React, { useEffect, useState } from "react";
import { BrowserRouter, Route, Routes, useLocation, useNavigate } from "react-router-dom";

import { Icon } from "./assets/icons/mdi";

import { AccountCard, Card, NavCard } from "./components/Card";

import { Home } from "./pages/HomePage";
import { Profile } from "./pages/ProfilePage";
import { Appointment } from "./pages/AppointmentPage";
import { Scheduler } from "./pages/SchedulerPage";

import "./App.css";
import { Login } from "./pages/LoginPage";
import { Register } from "./pages/RegisterPage";
import { ProfileEditor } from "./pages/ProfileEditorPage";
import { currentUser, currentUserImage, Patient } from "./services/patient";
import { getAccessToken, setAccessToken } from "./services/auth";
import { apiServer } from "./utils/api";
import { ProfileInfo } from "./pages/ProfileInfoPage";

function AppContent() {
  const location = useLocation();
  const navigate = useNavigate();
  const [user, setUser] = useState<Patient>();
  useEffect(() => {
    currentUser().then(value => {
      if (value.type === "ok") {
        setUser(value);
        return;
      }
      console.log("access_token", getAccessToken());
      setAccessToken();
      setUser(undefined);
      if (!["/", "/login", "/register"].includes(location.pathname))
        navigate("/login?redirect=" + encodeURIComponent(location.pathname + location.search + location.hash));
    }, reason => {
      console.log("access_token", getAccessToken());
      setAccessToken();
      setUser(undefined);
      if (["/", "/login", "/register"].includes(location.pathname))
        navigate("/login?redirect=" + encodeURIComponent(location.pathname + location.search + location.hash));
    });
  }, [getAccessToken(), navigate]);

  const [userImage, setUserImage] = useState<string>();
  useEffect(() => {
    currentUserImage().then(value => {
      if (value.type === "ok" && value.url) {
        setUserImage(value.url);
      } else setUserImage("/favicon.ico")
    }, reason => {
      setUserImage("/favicon.ico")
    });
  }, [getAccessToken(), navigate]);

  function wrapLogIn(url: string) {
    return user ? url : "/login?redirect=" + url;
  }

  return (
    <div className="App" mdc-theme="system">
      <Card heading="Phòng khám ABC" attributes={{
        style: { width: "100%" }
      }} />
      {(location.pathname.startsWith("/login")
        || location.pathname.startsWith("/register")
      ) ? '' : (<>
        <AccountCard
          user={user}
          name={user?.full_name}
          username={(user && (user.username || user.phone)) || "Bạn cần đăng nhập để thực hiện chức năng đặt lịch, xem lịch đã đặt, ..."}
          imageURL={userImage}
          actionAttributes={{
            async onClick(event: React.MouseEvent<HTMLElement, MouseEvent>) {
              event.preventDefault();
              if (user) {
                console.log("access_token", getAccessToken());
                setAccessToken();
                setUser(undefined);
                await navigate("/");
              } else await navigate("/login");
            }
          }} />
        <NavCard key={(user && user.id) || "_"} navigations={[
          { activeIcon: Icon.calendar_multiselect, icon: Icon.calendar_multiselect_outline, name: "Đặt lịch", url: wrapLogIn("/schedule") },
          { activeIcon: Icon.history, icon: Icon.history, name: "Lịch đã đặt", url: wrapLogIn("/appointment") },
          { activeIcon: Icon.account_box, icon: Icon.account_box_outline, name: "Hồ sơ", url: wrapLogIn("/profile") },
          { activeIcon: Icon.information, icon: Icon.information_outline, name: "Giới thiệu", url: "/" }
        ]} />
      </>)}
      <Routes>
        <Route index={!user} path="/" element={<Home />} />
        {!user ? '' : (<>
          <Route index={!!user} path="/profile" element={<Profile user={user} />} />
          <Route path="/profile/create" element={<ProfileEditor user={user} />} />
          <Route path="/profile/info" element={<ProfileInfo user={user} />} />
          <Route path="/appointment" element={<Appointment user={user} />} />
          <Route path="/schedule" element={<Scheduler user={user} />} />
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
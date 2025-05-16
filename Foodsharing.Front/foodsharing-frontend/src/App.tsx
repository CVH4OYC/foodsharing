// src/App.tsx
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./components/Layout";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Secure from "./pages/Secure";
import Profile from "./pages/Profile";
import ProfileAdsSection from "./components/ProfileAdsSection";
import AdsPage from "./pages/AdsPage";
import AdPage from "./pages/AdPage";
import AdFormPage from "./pages/AdFormPage";
import UserProfilePage from "./pages/UserProfilePage";
import { AuthProvider } from "./context/AuthContext";
import BusinessPage from "./pages/BusinessPage";
import PartnershipApplicationsPage from "./pages/PartnershipApplicationsPage";
import PartnershipApplicationDetailPage from "./pages/PartnershipApplicationDetailPage";

import RequireAdminRoute from "./components/RequireAdminRoute";
import OrganizationProfilePage from "./pages/OrganizationProfilePage";
import OrganizationAdminPage from "./pages/OrganizationAdminPage";
import OrgAnnouncementsPage from "./components/OrgAnnouncementsPage";
import OrgInfoPage from "./components/OrgInfoPage";
import OrgRepresentativesPage from "./components/OrgRepresentativesPage";
import ChatsPage from "./pages/ChatsPage";
import ChatWindow from "./components/chat/ChatWindow";
import ChatWindowPage from "./pages/ChatWindowPage";

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Layout />}>
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route index element={<AdsPage />} />
            <Route path="secure" element={<Secure />} />
            <Route path="ads" element={<AdsPage />} />
            <Route path="ads/new" element={<AdFormPage />} />
            <Route path="ads/edit/:announcementId" element={<AdFormPage />} />
            <Route path="ads/:announcementId" element={<AdPage />} />
            <Route path="/business" element={<BusinessPage />} />
            <Route path="/organizations/:orgId" element={<OrganizationProfilePage />} />
            <Route
              path="/admin/organizations/:orgId"
              element={
                <RequireAdminRoute>
                  <OrganizationAdminPage />
                </RequireAdminRoute>
              }
            >
              <Route path="announcements" element={<OrgAnnouncementsPage />} />
              <Route path="info" element={<OrgInfoPage />} />
              <Route path="representatives" element={<OrgRepresentativesPage />} />
            </Route>
            <Route
              path="/applications"
              element={
                <RequireAdminRoute>
                  <PartnershipApplicationsPage />
                </RequireAdminRoute>
              }
            />
            <Route path="/applications/:applicationId" element={
              <RequireAdminRoute>
                <PartnershipApplicationDetailPage />
              </RequireAdminRoute>
            }
            />

            <Route path="/chats" element={<ChatsPage />}>
              <Route path=":chatId" element={<ChatWindowPage />} />
              <Route path="new" element={<ChatWindowPage />} />
            </Route>
            {/* Профиль пользователя (личный и чужие профили) */}
            <Route path="profile" element={<Profile />}>
              <Route path="ads" element={<ProfileAdsSection />} />
            </Route>

            <Route path="profile/user/:userId" element={<UserProfilePage />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;

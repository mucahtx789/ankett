import { createRouter, createWebHistory } from 'vue-router';
import SurveyList from './components/SurveyList.vue';
import Login from './components/Login.vue';
import Register from './components/Register.vue';
import SurveyDetail from './components/SurveyDetail.vue';
import CreateSurvey from './components/CreateSurvey.vue';
import TakeSurvey from './components/TakeSurvey.vue';

// Token doğrulama fonksiyonu
function isTokenValid() {
  const token = localStorage.getItem('token');
  if (!token) return false;

  // Token'ı decode et ve geçerliliğini kontrol et
  const payload = JSON.parse(atob(token.split('.')[1])); // Token'ı decode et
  const exp = payload.exp * 1000; // Expiry timestamp (milisaniye cinsinden)
  return Date.now() < exp; // Geçerli mi?
}

// Vue Router'ı tanımlayın
const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', redirect: '/login' },
    { path: '/login', component: Login },  // Login sayfası rotası
    { path: '/register', component: Register },
    { path: '/survey-list', component: SurveyList, meta: { requiresAuth: true } },
    { path: '/survey-detail/:id', component: SurveyDetail, meta: { requiresAdmin: true } },
    { path: '/create-survey', component: CreateSurvey, meta: { requiresAdmin: true } },
    { path: '/take-survey/:id', component: TakeSurvey, meta: { requiresAuth: true } },
  ],
});

// Navigation Guard
router.beforeEach((to, from, next) => {
  const isAuthenticated = !!localStorage.getItem('userId');
  const userRole = localStorage.getItem('userRole');
  const tokenValid = isTokenValid(); // Token doğrulama kontrolü

  // Eğer sayfa oturum gerektiriyorsa ve kullanıcı giriş yapmamışsa login sayfasına yönlendir
  if (to.meta.requiresAuth && !isAuthenticated) {
    alert('Bu sayfaya erişmek için giriş yapmalısınız!');
    return next('/login');
  }

  // Eğer token geçerli değilse, kullanıcıyı login sayfasına yönlendir
  if (to.meta.requiresAuth && !tokenValid) {
    alert('Oturum süreniz dolmuş. Lütfen tekrar giriş yapın!');
    localStorage.removeItem('token'); // Geçersiz token'ı kaldır
    localStorage.removeItem('userId');
    localStorage.removeItem('userRole');
    return next('/login');
  }

  // Eğer sayfa admin gerektiriyorsa ve kullanıcı admin değilse yönlendir
  if (to.meta.requiresAdmin && userRole !== 'Admin') {
    alert('Bu sayfaya sadece adminler erişebilir!');
    return next('/survey-list');
  }

  next(); // Tüm kontroller geçildiyse yönlendirmeye devam et
});

export default router;

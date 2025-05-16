import { createRouter, createWebHistory } from 'vue-router';
import SurveyList from './components/SurveyList.vue';
import Login from './components/Login.vue';
import Register from './components/Register.vue';
import SurveyDetail from './components/SurveyDetail.vue';
import CreateSurvey from './components/CreateSurvey.vue';
import TakeSurvey from './components/TakeSurvey.vue';

function isTokenValid() {
  const token = localStorage.getItem('token');
  if (!token) return false;

  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    const exp = payload.exp * 1000;
    return Date.now() < exp;
  } catch (e) {
    // Token hatalı veya parse edilemiyor
    return false;
  }
}

const routes = [
  { path: '/', redirect: '/login' },
  { path: '/login', component: Login },
  { path: '/register', component: Register },
  { path: '/survey-list', component: SurveyList, meta: { requiresAuth: true } },
  { path: '/survey-detail/:id', component: SurveyDetail, meta: { requiresAdmin: true } },
  { path: '/create-survey', component: CreateSurvey, meta: { requiresAdmin: true } },
  { path: '/take-survey/:id', component: TakeSurvey, meta: { requiresAuth: true } },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const isAuthenticated = !!localStorage.getItem('userId');
  const userRole = localStorage.getItem('userRole');
  const tokenValid = isTokenValid();

  if (to.meta.requiresAuth && !isAuthenticated) {
    alert('Bu sayfaya erişmek için giriş yapmalısınız!');
    return next('/login');
  }

  if (to.meta.requiresAuth && !tokenValid) {
    alert('Oturum süreniz dolmuş. Lütfen tekrar giriş yapın!');
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    localStorage.removeItem('userRole');
    return next('/login');
  }

  if (to.meta.requiresAdmin && userRole !== 'Admin') {
    alert('Bu sayfaya sadece adminler erişebilir!');
    return next('/survey-list');
  }

  next();
});

export default router;

import { createRouter, createWebHistory } from 'vue-router';
import SurveyList from './components/SurveyList.vue';
import Login from './components/Login.vue';
import Register from './components/Register.vue';
import SurveyDetail from './components/SurveyDetail.vue';
import CreateSurvey from './components/CreateSurvey.vue';
import TakeSurvey from './components/TakeSurvey.vue';

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

  // Eğer sayfa oturum gerektiriyorsa ve kullanıcı giriş yapmamışsa login sayfasına yönlendir
  if (to.meta.requiresAuth && !isAuthenticated) {
    alert('Bu sayfaya erişmek için giriş yapmalısınız!');
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

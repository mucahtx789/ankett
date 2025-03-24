import { createRouter, createWebHistory } from 'vue-router';
import Login from '../components/Login.vue'; // Login bileşenini import et
import SurveyList from '../components/SurveyList.vue'; // SurveyList bileşenini import et

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Login // Ana sayfa olarak Login bileşenini göster
  },
  {
    path: '/survey-list',
    name: 'SurveyList',
    component: SurveyList // SurveyList bileşenine yönlendir
  },
  // Diğer route'lar eklenebilir
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
});

export default router;

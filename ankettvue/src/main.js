import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import vueRecaptcha from 'vue3-recaptcha2';

const app = createApp(App);

// ReCaptcha bileşenini global olarak kaydediyoruz
app.component('vue-recaptcha', vueRecaptcha);

app.use(router);
app.mount('#app');

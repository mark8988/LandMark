import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import store from './state/store'; 
import router from "./router";
import i18n from './i18n';
import AOS from 'aos';
import 'aos/dist/aos.css';

import BootstrapVueNext from 'bootstrap-vue-next';

import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue-next/dist/bootstrap-vue-next.css'

AOS.init({
    easing: 'ease-out-back',
    duration: 1000
});

createApp(App)
    .use(store)
    .use(router)
    .use(i18n)
    .use(BootstrapVueNext)
    .mount('#app')

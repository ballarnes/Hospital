import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import translationEN from './en/translationEN.json';
import translationRU from './ru/translationRU.json';

export const resources = {
  en: {
    translation: translationEN
  },
  ru: {
    translation: translationRU
  }
} as const;

i18n.use(initReactI18next).init({
  resources,
  lng: 'en',
  ns: ['translation'],
  interpolation: {
    escapeValue: false, // not needed for react as it escapes by default
  }
});


export default i18n;
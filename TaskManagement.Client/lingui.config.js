import { formatter } from "@lingui/format-json";

/** @type {import('@lingui/conf').LinguiConfig} */
export default {
  locales: ["en", "es"],
  sourceLocale: "en",
  catalogs: [
    {
      path: "src/locales/{locale}/messages",
      include: ["src"]
    }
  ],
  format: formatter({ style: "minimal" }),
  orderBy: "messageId"
};

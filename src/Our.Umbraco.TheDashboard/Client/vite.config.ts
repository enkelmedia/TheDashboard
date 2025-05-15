import { defineConfig } from "vite";

export default defineConfig(({ command }) => {

  const buildMode = process.env.build_mode?.trim();

  console.log('vite.config.ts | env.buildMode',buildMode);

  return {
    build: {
        lib: {
            entry: "src/entry.ts",
            formats: ["es"],
            name : "TheDashboard.Umbraco"
        },
        outDir: "../wwwroot/App_Plugins/Our.Umbraco.TheDashboard/dist",
        emptyOutDir: true,
        sourcemap: buildMode == 'development' ? true : false,
        rollupOptions: {
            external: [/^@umbraco/]
        },
    }
  }
});

import { createFileRoute } from "@tanstack/react-router";
import { useEffect } from "react";
import { toast } from "sonner";

import { switchToMenuMode } from "@/lib/runtime-api";

export const Route = createFileRoute("/play-mode/")({
  component: RouteComponent,
});

function RouteComponent() {
  useEffect(
    () => {
      toast(
        "Galaxy Unleashed",
        {
          id: "play-mode-open-menu-toast",
          description: "Click F1 to open the menu.",
          position: "top-right",
        },
      );

      return () => {
        toast.getHistory()
          .map(x => x.id)
          .filter(toastId => typeof toastId === `string` && toastId.startsWith(`play-mode-`))
          .forEach(toast.dismiss);
      };
    },
    [],
  );

  return (
    <div
      className="h-screen w-screen"
      onClick={async () => {
        await switchToMenuMode({
          menuModeConfig: {
            navigateParams: {
              to: `/menu-mode/menu`,
              search: null,
            },
          },
        });
      }}
    />
  );
}
